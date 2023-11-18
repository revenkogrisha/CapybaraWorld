using System.Threading;
using Core.Common;
using Core.Infrastructure;
using Core.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Level
{
	public class EnemyIdleState : State
	{
		private readonly Transform _thisTransform;
		private readonly ChaseEnemy _enemy;
		private LookingDirection _direction;
		private CancellationTokenSource _cancellationSource;

		public EnemyIdleState(ChaseEnemy enemy)
		{
			_enemy = enemy;
			_thisTransform = enemy.transform;
		}

		public override void Enter()
		{
			StopMoving();
			StartLookingForTarget().Forget();
		}

		public override void Exit()
		{
			_cancellationSource?.Cancel();
			_cancellationSource?.Dispose();
			_cancellationSource = null;
		}
		
		private void StopMoving()
		{
			Vector2 velocity = Vector2.zero;
			_enemy.Rigidbody2D.velocity = velocity;
		}

		private async UniTaskVoid StartLookingForTarget()
		{
			_cancellationSource = new();
			CancellationToken token = _cancellationSource.Token;

			bool canceled = false;
			while (canceled == false)
			{
				Vector2 origin = _thisTransform.position;
				float spotRadius = _enemy.Config.SpotRadius;
				LayerMask targetLayer = _enemy.Config.TargetLayer;
				Vector2 direction = Vector2.left * (float)_direction;

				RaycastHit2D hit = Physics2D.Raycast(origin, direction, spotRadius, targetLayer);
				if (hit == true)
				{
					Vector2 targetPosition = hit.transform.position;
					FiniteStateMachine.ChangeState<EnemyChaseState, Vector2>(targetPosition);
					break;
				}

				ChangeDirection();

				canceled = await MyUniTask
					.Delay(_enemy.Config.SpotInterval, token)
					.SuppressCancellationThrow();
			}

			_cancellationSource?.Dispose();
			_cancellationSource = null;
		}

		private void ChangeDirection()
		{
			_direction = _direction == LookingDirection.Left
				? LookingDirection.Right
				: LookingDirection.Left;
		}
	}
}
