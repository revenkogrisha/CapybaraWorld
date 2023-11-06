using System;
using System.Diagnostics;
using Core.Infrastructure;
using Core.Level;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityTools;

namespace Core.Player
{
    public class Hero : MonoBehaviour
    {
        private const float GrapplingActivationDistance = 5f;

        [Header("Components")]
        [SerializeField] private SpringJoint2D _springJoint2D;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GrapplingRope _rope;

        private PlayerConfig _config;
        private IFiniteStateMachine _stateMachine;
        private MiddleObject _middleObject;

        public readonly ReactiveProperty<bool> IsDead = new(false);

        private readonly CompositeDisposable _disposable = new();
        public SpringJoint2D SpringJoint2D => _springJoint2D;
        public LineRenderer LineRenderer => _lineRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public MiddleObject MiddleObject => _middleObject;
        public PlayerConfig Config => _config;
        public GrapplingRope Rope => _rope;

        public event Action<Transform> JointGrappled;
        public event Action JointReleased;

        private bool HaveGroundBelow
        {
            get
            {
                var hit = Physics2D.Raycast(
                    transform.position,
                    Vector2.down,
                    GrapplingActivationDistance,
                    _config.GroundLayer);

                return hit;
            }
        }


        #region MonoBehaviour

        private void Awake()
        {
            _springJoint2D.enabled = false;
            _lineRenderer.enabled = false;
            InitialzeStateMachine();
        }

        private void Start()
        {
            SubscribeUpdate();
            SubscribePhysicsCallbacks();
        }

        private void OnDisable()
        {
            _disposable.Clear();

            if (_middleObject != null)
            {
                _middleObject = null;
                Destroy(_middleObject);
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            if (_config == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _config.GrappleRadius);
        }

        #endregion

        public void Initialize(PlayerConfig config, MiddleObject middleObject)
        {
            _config = config;
            _middleObject = middleObject;
        }

        private void SubscribeUpdate()
        {
            IObservable<Unit> update = this.UpdateAsObservable();
            update
                .Where(_ => HaveGroundBelow == false)
                .Subscribe(_ => SwitchToGrapplingState())
                .AddTo(_disposable);
        }

        private void SubscribePhysicsCallbacks()
        {
            IObservable<Collider2D> onTriggerEnter2D = this.OnTriggerEnter2DAsObservable();
            onTriggerEnter2D
                .Where(collider => collider.HasComponent<DeadlyForPlayerObject>() == true)
                .Subscribe(_ => PerformDeath())
                .AddTo(_disposable);

            IObservable<Collision2D> onCollisionEnter2D = this.OnCollisionEnter2DAsObservable();
            onCollisionEnter2D
                .Where(collision => collision.CompareLayers(_config.GroundLayer) == true)
                .Subscribe(_ => SwitchToRunState())
                .AddTo(_disposable);
        }

        public void NotifyOnJointGrappled() =>
            JointGrappled?.Invoke(_middleObject.transform);

        public void NotifyOnJointReleased() =>
            JointReleased?.Invoke();

        private void InitialzeStateMachine()
        {
            _stateMachine = new FiniteStateMachine();

            HeroGrapplingState grapplingState = new(this);
            HeroRunState runState = new(this);

            _stateMachine.AddState<HeroGrapplingState>(grapplingState);
            _stateMachine.AddState<HeroRunState>(runState);
        }

        private void SwitchToGrapplingState() => 
            _stateMachine.ChangeState<HeroGrapplingState>();

        private void SwitchToRunState() =>
            _stateMachine.ChangeState<HeroRunState>();

        private void PerformDeath() => 
            IsDead.Value = true;
    }
}
