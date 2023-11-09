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
        private bool _shouldRotateHand;
        private bool _shouldRotateBody;

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
            _hero.JointGrappled += StartRotatingBody;
            _hero.JointGrappled += StartRotatingHand;
            
            _hero.JointReleased += StopRotatingBody;
            _hero.JointReleased += StopRotatingHand;
        }

        private void OnDisable()
        {
            _hero.JointGrappled -= StartRotatingBody;
            _hero.JointGrappled -= StartRotatingHand;

            _hero.JointReleased -= StopRotatingBody;
            _hero.JointReleased -= StopRotatingHand;
        }

        #endregion

        private async void StartRotatingBody(Transform targetJoint)
        {
            CancellationToken token = destroyCancellationToken;
            _shouldRotateBody = true;

            while (_shouldRotateBody == true)
            {
                _thisTransform.rotation = LerpRotate(
                    _thisTransform,
                    targetJoint,
                    _config.BodyRotationSpeed);

                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }

        private async void StartRotatingHand(Transform targetJoint)
        {
            CancellationToken token = destroyCancellationToken;
            _shouldRotateHand = true;

            while (_shouldRotateHand == true)
            {
                _armWithHook.rotation = LerpRotate(
                    _armWithHook,
                    targetJoint,
                    _config.HandRotationSpeed);

                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }

        private Quaternion LerpRotate(Transform target, Transform targetJoint, float speed)
        {
            Vector2 direction = targetJoint.position - target.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            
            float delta = Time.deltaTime * speed;
            return Quaternion.Slerp(
                target.rotation,
                quaternion,
                delta);
        }

        private void StopRotatingHand()
        {
            _shouldRotateHand = false;
            LerpArmToDefaultAsync();
        }

        private void StopRotatingBody()
        {
            _shouldRotateBody = false;
            LerpBodyToDefatultAsync();
        }

        private void LerpArmToDefaultAsync() =>
            LerpToDefault(_armWithHook).Forget();

        private void LerpBodyToDefatultAsync() =>
            LerpToDefault(_thisTransform).Forget();

        private async UniTaskVoid LerpToDefault(Transform target)
        {
            CancellationToken token = destroyCancellationToken;

            float elapsedTime = 0f;
            while (elapsedTime < _config.RotateToDefaultDuration)
            {
                float delta = elapsedTime / _config.RotateToDefaultDuration;
                target.rotation = Quaternion.Slerp(
                    target.rotation,
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