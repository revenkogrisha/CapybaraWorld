using System;
using System.Threading;
using Core.Common;
using Core.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Level
{
    public class EnemySearchingState : State 
	{
		private readonly Transform _thisTransform;
        private readonly State<Vector2> _stateOnTrigger;
        private readonly SearchEnemyBase _enemy;
		private LookingDirection _direction;
		private CancellationTokenSource _cts;

		public EnemySearchingState(SearchEnemyBase enemy, State<Vector2> stateOnTrigger)
		{
			_enemy = enemy;
			_thisTransform = enemy.transform;
			_stateOnTrigger = stateOnTrigger;
		}

		public override void Enter()
		{
			_enemy.Triggered.Value = false;
			StartLookingForTarget().Forget();
		}

        public override void Exit() => 
			_cts.Clear();

        private async UniTaskVoid StartLookingForTarget()
		{
			_cts = new();
			CancellationToken token = _cts.Token;

			try
			{
				while (true)
				{
					RaycastHit2D hit = PerformSearchRaycast();
					if (hit == true)
					{
						Vector2 targetPosition = hit.transform.position;
						FiniteStateMachine.ChangeState(_stateOnTrigger, targetPosition);
						break;
					}

					ChangeDirection();

					await MyUniTask.Delay(_enemy.SearchPreset.SpotInterval, token);
				}
			}
			catch (Exception e)
			{
				Debug.Log($"{GetType()}: {e.Message} \n {e.StackTrace}");
			}
		}

		private void ChangeDirection()
		{
			_direction = _direction == LookingDirection.Left
				? LookingDirection.Right
				: LookingDirection.Left;
		}

		private RaycastHit2D PerformSearchRaycast()
		{
			Vector2 origin = _thisTransform.position;
			float spotRadius = _enemy.SearchPreset.SpotRadius;
			LayerMask targetLayer = _enemy.SearchPreset.TargetLayer;

			Vector2 direction = GetDirection();

			return Physics2D.Raycast(
				origin,
				direction,
				spotRadius,
				targetLayer);
		}

		private Vector2 GetDirection() =>
			Vector2.left * (float)_direction;
    }
}