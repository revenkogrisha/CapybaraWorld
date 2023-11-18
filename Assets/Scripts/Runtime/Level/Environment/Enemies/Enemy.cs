using Core.Infrastructure;
using UniRx;
using UnityEngine;

namespace Core.Level
{
	public class Enemy : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private EnemyConfig _config;

		private readonly CompositeDisposable _disposable = new();
		private IFiniteStateMachine _stateMachine;

		public Rigidbody2D Rigidbody2D => _rigidbody2D;
		public EnemyConfig Config => _config;

		#region MonoBehaviour

		private void Awake()
		{
			InitializeStateMachine();   
			_stateMachine.ChangeState<EnemyIdleState>();
		}

		private void OnDisable() => 
			_disposable.Clear();

		#endregion
		
		public void PerformDeath()
		{
			Destroy(gameObject);
		}

		private void InitializeStateMachine()
		{
			_stateMachine = new FiniteStateMachine();
			EnemyIdleState idleState = new(this);
			EnemyChaseState chaseState = new(this);

			_stateMachine.AddState<EnemyIdleState>(idleState);
			_stateMachine.AddState<EnemyChaseState>(chaseState);
		}
	}
}