using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Player
{
    public class FocusCamera : MonoBehaviour, IPlayerCamera
    {

        [SerializeField] private CinemachineVirtualCamera _cinemachine;
        
        [Space]
        [SerializeField] private CameraConfig _config;

        private Transform _followObject;
        private bool _focusing = false;
        private Hero _hero;
        private CancellationToken _cancellationToken;

        private float Fov
        {
            get => _cinemachine.m_Lens.OrthographicSize;
            set => _cinemachine.m_Lens.OrthographicSize = value;
        }

        private void OnDisable()
        {
            if (_hero != null)
            {
                _hero.JointGrappled -= StartFocus;
                _hero.JointReleased -= StopFocus;
            }
        }

        public void Initialize(Hero hero)
        {
            _hero = hero;
            _followObject = hero.transform;
            _cinemachine.Follow = _followObject;

            _cancellationToken = this.GetCancellationTokenOnDestroy();
            
            _hero.JointGrappled += StartFocus;
            _hero.JointReleased += StopFocus;
        }

        public void StartFocus(Transform toFocus)
        {
            if (_focusing == true)
                return;

            _focusing = true;
            _cinemachine.Follow = toFocus;
            ChangeFov(_config.FocusFov, _config.FovChangeDuration).Forget();
        }

        public void StopFocus()
        {
            if (_focusing == false)
                return;

            _focusing = false;
            _cinemachine.Follow = _followObject;
            ChangeFov(_config.RegularFov, _config.FovChangeDuration).Forget();
        }

        private async UniTask ChangeFov(
            float targetFov,
            float duration,
            CancellationToken token = default)
        {
            if (token == default)
                token = _cancellationToken;

            float elapsedTime = 0f;
            float currentFov = Fov;
            while (elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                Fov = Mathf.Lerp(currentFov, targetFov, delta);

                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame(token);
            }
        }
    }
}