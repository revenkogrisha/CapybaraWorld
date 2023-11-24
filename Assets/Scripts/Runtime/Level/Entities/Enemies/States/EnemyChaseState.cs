using System.Threading;
using Core.Infrastructure;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Core.Level
{
	public class EnemyChaseState : State<Vector2>
	{
		private const float TargetMinimumDistance = 0.5f;
		
		private readonly CompositeDisposable _disposable = new();
		private readonly Transform _thisTransform;
		private readonly ChaseEnemy _enemy;
		private Vector3 _targetPosition;
		private Vector2 _directionToTarget;

		public EnemyChaseState(ChaseEnemy enemy)
		{
			_enemy = enemy;
			_thisTransform = _enemy.transform;
		}

		public override void SetArg(Vector2 arg)
		{
			_targetPosition = arg;
			_directionToTarget = GetDirectionToTarget();
		}

        public override void Enter()
        {
			_enemy.Triggered.Value = true;
            ChaseTarget().Forget();
        }

        public override void Exit() => 
			_disposable.Clear();

        private async UniTaskVoid ChaseTarget()
		{
			CancellationToken token = _enemy.destroyCancellationToken;
			
			float elapsedTime = 0f;
			while (true)
			{
				float distance = GetDistanceToTarget();
				if (distance <= TargetMinimumDistance)
				{
					FiniteStateMachine.ChangeState<EnemySearchingState>();
					break;
				}
				
				Rigidbody2D rigidbody2D = _enemy.Rigidbody2D;
				Vector2 velocity = rigidbody2D.velocity;
				velocity.x = _directionToTarget.x * _enemy.Preset.Speed;
				
				rigidbody2D.velocity = velocity;
				
				elapsedTime += Time.deltaTime;
				await UniTask.NextFrame(token);
			}
		}

		private float GetDistanceToTarget() =>
			Vector2.Distance(_thisTransform.position, _targetPosition);

		private Vector2 GetDirectionToTarget() =>
			(_targetPosition - _thisTransform.position).normalized;
	}
}
