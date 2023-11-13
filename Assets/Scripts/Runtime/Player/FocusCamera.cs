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

        public void Dispose() => 
            _disposable.Clear();
        
        #endregion

        public void Initialize(Hero hero)
        {
            _hero = hero;
            _heroTransform = _hero.transform;

            _followObject = hero.transform;
            _cinemachine.Follow = _followObject;
            
            SubscribeHero();
            SubscribeUpdate();
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

        private void SubscribeHero()
        {
            _hero.GrappledJoint
                .Where(joint => joint != null)
                .Subscribe(joint => StartFocus(joint))
                .AddTo(_disposable);

            _hero.GrappledJoint
                .Where(joint => joint == null)
                .Subscribe(joint => StopFocus())
                .AddTo(_disposable);
        }

        private void SubscribeUpdate()
        {
            IObservable<Unit> update = this.UpdateAsObservable();
            update
                .Where(_ => _hero != null)
                .Subscribe(_ => SetHeroMiddleObject())
                .AddTo(_disposable);
        }

        private void SetHeroMiddleObject()
        {
            if (_jointTransform != null)
            {
                _heroJointMiddle.SetPosition(
                    _heroTransform.position,
                    _jointTransform.position);
            }
            else
            {
                _heroJointMiddle.transform.position = _heroTransform.position;
            }
        }

        private async UniTaskVoid ChangeFov(float targetFov, float duration)
        {
            CancellationToken token = destroyCancellationToken;
            float currentFov = CinemachineFov;

            float elapsedTime = 0f;

            bool canceled = false;
            while (canceled == false && elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                CinemachineFov = Mathf.Lerp(currentFov, targetFov, delta);
                
                elapsedTime += Time.deltaTime;
                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }
        }
    }
}