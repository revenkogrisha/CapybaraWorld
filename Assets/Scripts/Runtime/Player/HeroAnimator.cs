using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Player
{
    public class HeroAnimator : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Hero _hero;
        [SerializeField] private HeroAnimatorConfig _config;

        [Space]
        [SerializeField] private bool _enabled = true;

        [Header("Arm with Hook")]
        [SerializeField] private Transform _armWithHook;

        private Transform _thisTransform;
        private bool _shouldRotate;

        public bool Enabled 
        {
            get => _enabled;
            set => _enabled = value;
        }

        #region MonoBehaviour

        private void Awake() => 
            _thisTransform = transform;

        private void OnEnable()
        {
            _hero.JointGrappled += StartRotatingHand;
            _hero.JointReleased += StopRotatingHand;
        }

        private void OnDisable()
        {
            _hero.JointGrappled -= StartRotatingHand;
            _hero.JointReleased -= StopRotatingHand;
        }

        #endregion

        public async void StartRotatingHand(Transform target)
        {
            CancellationToken token = destroyCancellationToken;
            _shouldRotate = true;

            while (_shouldRotate == true)
            {
                Vector2 direction = target.position - _thisTransform.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
                
                float delta = Time.deltaTime * _config.RotationSpeed;
                _armWithHook.rotation = Quaternion.Slerp(
                    _armWithHook.rotation,
                    quaternion,
                    delta);

                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }

        public void StopRotatingHand()
        {
            _shouldRotate = false;
            LerpToDefault().Forget();
        }

        private async UniTaskVoid LerpToDefault()
        {
            CancellationToken token = destroyCancellationToken;

            float elapsedTime = 0f;
            while (elapsedTime < _config.RotateToDefaultDuration)
            {
                float delta = elapsedTime / _config.RotateToDefaultDuration;
                _armWithHook.rotation = Quaternion.Slerp(
                    _armWithHook.rotation,
                    _config.DefaultRotation,
                    delta);

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