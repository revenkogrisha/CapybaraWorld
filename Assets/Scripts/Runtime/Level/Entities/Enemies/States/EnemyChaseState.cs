using System;
using System.Threading;
using Core.Common;
using Core.Editor.Debugger;
using Core.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Level
{
    public class EnemyChaseState : State<Vector2>
	{
		private const float TargetMinimumDistance = 0.5f;
		
		private readonly Transform _thisTransform;
		private readonly ChaseEnemy _enemy;
		private CancellationTokenSource _cts;
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

        public override void Exit()
        {
			StopMoving();
			_cts.Clear();
			_cts = null;
        }

        private async UniTaskVoid ChaseTarget()
		{
			_cts = new();
			
			float elapsedTime = 0f;
			try
			{
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

					await UniTask.NextFrame(_cts.Token);
				}
			}
			catch (OperationCanceledException) {  }
			catch (Exception ex)
			{
				RDebug.Warning($"{nameof(EnemyChaseState)}::{nameof(ChaseTarget)}: {ex.Message} \n {ex.StackTrace}");
			}
		}

        private void StopMoving() => 
			_enemy.Rigidbody2D.velocity = Vector2.zero;

        private float GetDistanceToTarget() =>
			Vector2.Distance(_thisTransform.position, _targetPosition);

		private Vector2 GetDirectionToTarget() =>
			(_targetPosition - _thisTransform.position).normalized;
	}
}
