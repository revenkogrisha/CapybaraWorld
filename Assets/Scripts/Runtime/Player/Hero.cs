using System;
using System.Diagnostics;
using Core.Common;
using Core.Game.Input;
using Core.Level;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityTools;
using Core.Other;
using Zenject;
using TriInspector;
using Core.Editor.Debugger;
using DG.Tweening;
using Core.Audio;

namespace Core.Player
{
	[SelectionBase]
    public class Hero : MonoBehaviour, IDieable, IPlayerEventsHandler
	{
		private const float GrapplingActivationDistance = 50f;
		private const float JumpAllowedHeight = 1.1f;

		[Title("Components")]
		[SerializeField] private SpringJoint2D _springJoint2D;
		[SerializeField] private LineRenderer _lineRenderer;
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private GrapplingRope _rope;
		[SerializeField] private Collider2D _collider2D;

		[Title("Configuration")]
		[SerializeField] private HeroConfig _config;
        [SerializeField] private Vector3 _particlesOffset = Vector2.down;

		[Title("Death Tweening")]
        [SerializeField] private float _deathTweenDuration = 0.3f;

		private readonly CompositeDisposable _disposable = new();
		private IFiniteStateMachine _stateMachine;
		private GroundChecker _groundChecker;
		private InputHandler _inputHandler;
		private PlayerUpgrade _upgrade;
        private ParticlesHelper _particles;
        private IAudioHandler _audioHandler;

        [HideInInspector] public ReactiveProperty<bool> IsDead { get; } = new(false);
		[HideInInspector] public readonly ReactiveProperty<Transform> GrappledJoint = new();
		[HideInInspector] public readonly ReactiveProperty<bool> IsRunning = new();
		[HideInInspector] public readonly ReactiveProperty<bool> IsJumping = new();
		
		public readonly ReactiveCommand<Type> StateChangedCommand = new();
		public readonly ReactiveCommand<float> DashedCommand = new();
		public readonly ReactiveCommand HitCommand = new();
		public ReactiveCommand CoinCollectedCommand { get; } = new();
        public ReactiveCommand FoodCollectedCommand { get; } = new();

		public bool IsDashing { get; set; } = false;

		public SpringJoint2D SpringJoint2D => _springJoint2D;
		public Collider2D Collider2D => _collider2D;
		public LineRenderer LineRenderer => _lineRenderer;
		public Rigidbody2D Rigidbody2D => _rigidbody2D;
		public HeroConfig Config => _config;
		public GrapplingRope Rope => _rope;

		public bool CanJump => _groundChecker.GetHeight() <= JumpAllowedHeight;

		private bool ShouldSwitchToGrappling => _groundChecker.HaveGroundBelow() == false 
			&& _stateMachine.CompareState<HeroGrapplingState>() == false;

		private bool ShouldSwitchToRunning => _stateMachine.CompareState<HeroRunState>() == false;

		#region MonoBehaviour

		private void Awake()
		{
			InitializeComponents();
			InitializeStateMachine();

			SetGravityScale();

			SubscribeUpdate();
			SubscribePhysicsCallbacks();

			// Could be separated to HeroAudio audio for complexity. 
			DashedCommand	
				.Subscribe(_ => PlayDashSound())
				.AddTo(_disposable);

			FoodCollectedCommand
				.Subscribe(_ => PlayCollectSound())
				.AddTo(_disposable);

			IsJumping
				.Where(value => value == true)	
				.Subscribe(_ => PlayJumpSound())
				.AddTo(_disposable);

			GrappledJoint
				.Where(joint => joint != null)
				.Subscribe(_ => PlayGrappleSound())
				.AddTo(_disposable);
		}

        private void OnDestroy()
        {
			_stateMachine.ChangeState<InactiveState>();
            _disposable.Clear();
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

		[Inject]
		private void Construct(
			InputHandler inputHandler,
			PlayerUpgrade upgrade,
			ParticlesHelper particles,
			IAudioHandler audioHandler)
		{
			_inputHandler = inputHandler;
			_upgrade = upgrade;
			_particles = particles;
			_audioHandler = audioHandler;
		}

        public void PlayJumpParticles()
        {
            _particles
				.Spawn(_config.JumpParticlesName, transform.position + _particlesOffset)
				.Forget();
        }

        private void InitializeComponents()
		{
			LayerMask groundLayer = _config.GroundLayer;
			_groundChecker = new(transform, GrapplingActivationDistance, groundLayer);
			_springJoint2D.enabled = false;
			_lineRenderer.enabled = false;
		}

		private void InitializeStateMachine()
		{
			_stateMachine = new FiniteStateMachine();

			HeroGrapplingState grapplingState = new(this, _inputHandler);
			HeroRunState runState = new(this, _inputHandler, _upgrade);
			InactiveState inactiveState = new();

			_stateMachine.AddState<HeroGrapplingState>(grapplingState);
			_stateMachine.AddState<HeroRunState>(runState);
			_stateMachine.AddState<InactiveState>(inactiveState);
		}

		private void SetGravityScale()
		{
			if (_config.DefaultGravityScale != _rigidbody2D.gravityScale)
				RDebug.Warning($"{nameof(Hero)}: Gravity scale differs in config and in rigidbody component on object.");
				
			_rigidbody2D.gravityScale = _config.DefaultGravityScale;
		}

		private void SubscribeUpdate()
		{
			this.UpdateAsObservable()
				.Where(_ => ShouldSwitchToGrappling == true)
				.Subscribe(_ => SwitchToGrapplingState())
				.AddTo(_disposable);
		}

		private void SubscribePhysicsCallbacks()
		{
			IObservable<Collider2D> onTriggerEnter2D = this.OnTriggerEnter2DAsObservable();
			onTriggerEnter2D
				.Subscribe(collider => 
					Tools.InvokeIfNotNull<DeadlyForPlayerObject>(collider, PerformDeath))
				.AddTo(_disposable);

			onTriggerEnter2D
				.Where(_ => IsDashing == true)
				.Subscribe(collider => 
					Tools.InvokeIfNotNull<Chest>(collider, chest => chest.Open()))
				.AddTo(_disposable);

			onTriggerEnter2D
				.Subscribe(collider => 
					Tools.InvokeIfNotNull<Food>(collider, TryCollectFood))
				.AddTo(_disposable);

			IObservable<Collision2D> onCollisionEnter2D = this.OnCollisionEnter2DAsObservable();
			onCollisionEnter2D
				.Where(_ => ShouldSwitchToRunning == true)
				.Where(collision => collision.CompareLayers(_config.GroundLayer) == true)
				.Subscribe(_ => SwitchToRunState())
				.AddTo(_disposable);

			onCollisionEnter2D
				.Where(_ => IsDashing == true)
				.Subscribe(collision => 
					Tools.InvokeIfNotNull<Enemy>(collision, OnDashIntoEnemy))
				.AddTo(_disposable);

			onCollisionEnter2D
				.Where(_ => IsDashing == false) 
				.Subscribe(collision => 
					Tools.InvokeIfNotNull<Enemy>(collision, OnEnemyCollision))
				.AddTo(_disposable);
			
			onCollisionEnter2D
				.Subscribe(collision => 
					Tools.InvokeIfNotNull<DeadlyForPlayerObject>(collision, PerformDeath))
				.AddTo(_disposable);

			onCollisionEnter2D
				.Subscribe(collision => 
					Tools.InvokeIfNotNull<Coin>(collision, TryCollectCoin))
				.AddTo(_disposable);
		}

        private void PlayDashSound() => 
			_audioHandler.PlaySound(AudioName.HeroDash);

		private void PlayCollectSound() =>
			_audioHandler.PlaySound(AudioName.HeroCollect);

        private void PlayJumpSound() => 
			_audioHandler.PlaySound(AudioName.HeroJump);

        private void PlayGrappleSound() => 
			_audioHandler.PlaySound(AudioName.HeroGrapple);

        private void SwitchToGrapplingState()
		{
			_stateMachine.ChangeState<HeroGrapplingState>();
			StateChangedCommand.Execute(typeof(HeroGrapplingState));

			// Called here for simplicity. Should be in special class-manager or ~HeroFeedbackHalder
            HapticHelper.VibrateLight();
		}

		private void SwitchToRunState()
		{
			_stateMachine.ChangeState<HeroRunState>();
			StateChangedCommand.Execute(typeof(HeroRunState));
			
			// Called here for simplicity. Should be in special class-manager or ~HeroFeedbackHalder
            HapticHelper.VibrateLight();
		}

		private void OnEnemyCollision(Enemy enemy)
		{
			if (enemy.IsDead.Value == false)
				PerformDeath();
		}

		private void PerformDeath()
		{
			_stateMachine.ChangeState<InactiveState>();

            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, _deathTweenDuration))
                .AppendCallback(() => IsDead.Value = true);
		}

		private void OnDashIntoEnemy(Enemy enemy)
		{
			bool isDefeated = enemy.TryPerformDeath();

			if (isDefeated == false)
			{
				PerformDeath();
				return;
			}
			
			HitCommand.Execute();
		}

		private void TryCollectCoin(Coin coin)
		{
			if (coin.CanCollect == false)
				return;

			coin.OnCollected();
			CoinCollectedCommand.Execute();
		}

		private void TryCollectFood(Food food)
		{
			if (food.CanCollect == false)
				return;

			food.OnCollected();
			FoodCollectedCommand.Execute();
		}
	}
}