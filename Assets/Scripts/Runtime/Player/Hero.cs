using System;
using System.Diagnostics;
using Core.Common;
using Core.Game.Input;
using Core.Infrastructure;
using Core.Level;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityTools;
using Zenject;

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

        [Header("Config")]
        [SerializeField] private PlayerConfig _config;

        private readonly CompositeDisposable _disposable = new();
        private IFiniteStateMachine _stateMachine;
        private GroundChecker _groundChecker;
        private InputHandler _inputHandler;

        public readonly ReactiveProperty<Transform> GrappledJoint = new();
        public readonly ReactiveProperty<bool> IsRunning = new();
        public readonly ReactiveCommand DashedCommand = new();
        public readonly ReactiveCommand<Type> StateChangedCommand = new();
        public ReactiveProperty<bool> IsDead { get; private set; } = new(false);

        public SpringJoint2D SpringJoint2D => _springJoint2D;
        public LineRenderer LineRenderer => _lineRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public PlayerConfig Config => _config;
        public GrapplingRope Rope => _rope;

        private bool ShouldSwitchToGrappling => _groundChecker.HaveGroundBelow() == false 
            && _stateMachine.CompareState<HeroGrapplingState>() == false;

        #region MonoBehaviour

        private void Awake()
        {
            InitializeComponents();
            InitialzeStateMachine();
        }

        private void Start()
        {
            SubscribeUpdate();
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

        [Inject]
        private void Construct(InputHandler inputHandler) =>
            _inputHandler = inputHandler;

        private void InitializeComponents()
        {
            LayerMask groundLayer = _config.GroundLayer;
            _groundChecker = new(transform, GrapplingActivationDistance, groundLayer);
            _springJoint2D.enabled = false;
            _lineRenderer.enabled = false;
        }

        private void InitialzeStateMachine()
        {
            _stateMachine = new FiniteStateMachine();

            HeroGrapplingState grapplingState = new(this, _inputHandler);
            HeroRunState runState = new(this, _inputHandler);

            _stateMachine.AddState<HeroGrapplingState>(grapplingState);
            _stateMachine.AddState<HeroRunState>(runState);
        }

        private void SubscribeUpdate()
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

        private void SwitchToGrapplingState()
        {
            _stateMachine.ChangeState<HeroGrapplingState>();
            StateChangedCommand.Execute(typeof(HeroGrapplingState));
        }

        private void SwitchToRunState()
        {
            _stateMachine.ChangeState<HeroRunState>();
            StateChangedCommand.Execute(typeof(HeroRunState));
        }

        private void PerformDeath() => 
            IsDead.Value = true;
    }
}