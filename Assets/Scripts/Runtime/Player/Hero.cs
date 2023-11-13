using System;
using System.Diagnostics;
using Core.Infrastructure;
using Core.Level;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityTools;

namespace Core.Player
{
    public class Hero : MonoBehaviour, IDieable
    {
        private const float GrapplingActivationDistance = 5f;

        [Header("Components")]
        [SerializeField] private SpringJoint2D _springJoint2D;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GrapplingRope _rope;

        private readonly CompositeDisposable _disposable = new();
        private PlayerConfig _config;
        private Transform _thisTransform;
        private IFiniteStateMachine _stateMachine;

        public readonly ReactiveProperty<Transform> GrappledJoint = new();
        public readonly ReactiveProperty<bool> IsRunning = new();
        public readonly ReactiveCommand DashEndedCommand = new();
        public ReactiveProperty<bool> IsDead { get; private set; } = new(false);

        public SpringJoint2D SpringJoint2D => _springJoint2D;
        public LineRenderer LineRenderer => _lineRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public PlayerConfig Config => _config;
        public GrapplingRope Rope => _rope;

        public event Action<Type> StateChanged;

        private bool ShouldSwitchToGrappling => HaveGroundBelow == false 
            && _stateMachine.CompareState<HeroGrapplingState>() == false;

        private bool HaveGroundBelow
        {
            get
            {
                return Physics2D.Raycast(
                    _thisTransform.position,
                    Vector2.down,
                    GrapplingActivationDistance,
                    _config.GroundLayer);
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            _springJoint2D.enabled = false;
            _lineRenderer.enabled = false;
            _thisTransform = transform;
            InitialzeStateMachine();
        }

        private void Start()
        {
            SubscribeProperties();
            SubscribePhysicsCallbacks();
        }

        private void OnDestroy() => 
            _disposable.Clear();

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            if (_config == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _config.GrappleRadius);
        }

        #endregion

        public void Initialize(PlayerConfig config) => 
            _config = config;

        private void SubscribeProperties()
        {
            IObservable<Unit> update = this.UpdateAsObservable();
            update
                .Where(_ => ShouldSwitchToGrappling == true)
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

        private void InitialzeStateMachine()
        {
            _stateMachine = new FiniteStateMachine();

            HeroGrapplingState grapplingState = new(this);
            HeroRunState runState = new(this);

            _stateMachine.AddState<HeroGrapplingState>(grapplingState);
            _stateMachine.AddState<HeroRunState>(runState);
        }

        private void SwitchToGrapplingState()
        {
            _stateMachine.ChangeState<HeroGrapplingState>();
            StateChanged?.Invoke(typeof(HeroGrapplingState));
        }

        private void SwitchToRunState()
        {
            _stateMachine.ChangeState<HeroRunState>();
            StateChanged?.Invoke(typeof(HeroRunState));
        }

        private void PerformDeath() => 
            IsDead.Value = true;
    }
}
