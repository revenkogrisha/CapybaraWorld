using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Core.Player
{
    public class HeroAnimator : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private Hero _hero;
        [SerializeField] private Animator _animator;

        [Header("General")]
        [SerializeField] private bool _enabled = true;
        [SerializeField] private HeroAnimatorConfig _config;
        
        [Header("Arm with Hook")]
        [SerializeField] private Transform _armWithHook;

        [Header("Legs")]
        [SerializeField] private Transform[] _legs;

        private readonly int FreeFallingHash = Animator.StringToHash("FreeFalling");

        private CompositeDisposable _disposable = new();
        private Transform _thisTransform;
        private bool _shouldRotateHand;
        private bool _shouldRotateBody;

        public bool Enabled 
        {
            get => _enabled;
            set => _enabled = value;
        }

        public bool HeroRaising => 
            _hero.Rigidbody2D.velocity.y > _config.HeroRaisingVelocityMinimum;

        public bool HeroFalling => 
            _hero.Rigidbody2D.velocity.y < _config.HeroFallingVelocityMinimum;

        #region MonoBehaviour

        private void Awake() => 
            _thisTransform = transform;

        private void Start() =>
            SubscribeUpdate();

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

        private void SubscribeUpdate()
        {
            IObservable<Unit> update = this.UpdateAsObservable();
            update
                .Where(_ => HeroRaising == true)
                .Subscribe(_ => RotateLegsRaising())
                .AddTo(_disposable);

            update
                .Where(_ => HeroFalling == true)
                .Subscribe(_ => RotateLegsFalling())
                .AddTo(_disposable);
        }

        private async void StartRotatingBody(Transform targetJoint)
        {
            if (_config.RotateBody == false)
                return;

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
            if (_config.RotateArmWithHook == false)
                return;

            _animator.enabled = false;

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

            _animator.enabled = true;
            _animator.SetBool(FreeFallingHash, true);
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

        private void RotateLegsRaising()
        {
            if (_config.RotateLegs == false)
                return;

            foreach (Transform leg in _legs)
            {
                float offset = Random.Range(0f, 0.5f);
                leg.DORotate(_config.RaisingLegsRotation, _config.LegsRotationDuration + offset);
            }
        }

        private void RotateLegsFalling()
        {
            if (_config.RotateLegs == false)
                return;

            foreach (Transform leg in _legs)
            {
                float offset = Random.Range(0f, 0.5f);
                leg.DORotate(_config.FallingLegsRotation, _config.LegsRotationDuration + offset);
            }
        }
    }
}