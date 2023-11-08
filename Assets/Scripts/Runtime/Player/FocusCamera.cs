using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Player
{
    public class FocusCamera : MonoBehaviour, IPlayerCamera
    {
        [Header("Components")]
        [SerializeField] private CinemachineVirtualCamera _cinemachine;
        [SerializeField] private MiddleObject _heroJointMiddle;
        
        [Space]
        [SerializeField] private CameraConfig _config;

        private CompositeDisposable _disposable = new();
        private Transform _followObject;
        private bool _focusing = false;
        private Hero _hero;
        private Transform _jointTransform;
        private Transform _heroTransform;

        private float CinemachineFov
        {
            get => _cinemachine.m_Lens.OrthographicSize;
            set => _cinemachine.m_Lens.OrthographicSize = value;
        }

        #region MonoBehaviour
        
        private void Start()
        {
            IObservable<Unit> update = this.UpdateAsObservable();
            update
                .Where(_ => _hero != null)
                .Subscribe(_ => SetHeroMiddleObject())
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            if (_hero != null)
            {
                _hero.JointGrappled -= StartFocus;
                _hero.JointReleased -= StopFocus;
            }
        }
        
        #endregion

        public void Initialize(Hero hero)
        {
            _hero = hero;
            _heroTransform = _hero.transform;

            _followObject = hero.transform;
            _cinemachine.Follow = _followObject;
            
            _hero.JointGrappled += StartFocus;
            _hero.JointReleased += StopFocus;
        }

        public void StartFocus(Transform joint)
        {
            if (_focusing == true)
                return;

            _jointTransform = joint;

            _focusing = true;
            _cinemachine.Follow = _heroJointMiddle.transform;

            ChangeFov(_config.FocusFov, _config.FovChangeDuration)
                .Forget();
        }

        public void StopFocus()
        {
            if (_focusing == false)
                return;

            _jointTransform = null;
            _focusing = false;
            _cinemachine.Follow = _followObject;

            ChangeFov(_config.RegularFov, _config.FovChangeDuration)
                .Forget();
        }

        private void SetHeroMiddleObject()
        {
            if (_jointTransform != null)
                _heroJointMiddle.SetPosition(
                    _heroTransform.position,
                    _jointTransform.position);
            else
                _heroJointMiddle.transform.position = _heroTransform.position;
        }

        private async UniTaskVoid ChangeFov(float targetFov, float duration)
        {
            CancellationToken token = destroyCancellationToken;
            float currentFov = CinemachineFov;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                CinemachineFov = Mathf.Lerp(currentFov, targetFov, delta);

                elapsedTime += Time.deltaTime;
                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }
    }
}