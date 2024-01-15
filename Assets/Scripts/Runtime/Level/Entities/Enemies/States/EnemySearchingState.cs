using System;
using System.Threading;
using Core.Common;
using Core.Editor.Debugger;
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

        public override void Exit()
        {
	        _cts.Clear();
	        _cts = null;
        }

        private async UniTaskVoid StartLookingForTarget()
		{
			_cts = new();

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

					await UniTaskUtility.Delay(_enemy.SearchPreset.SpotInterval, _cts.Token);
				}
			}
			catch (OperationCanceledException) {  }
			catch (Exception ex)
			{
				RDebug.Warning($"{nameof(EnemySearchingState)}::{nameof(StartLookingForTarget)}: {ex.Message} \n{ex.StackTrace}");
			}
		}

		private void ChangeDirection()
		{
			_enemy.Direction = _enemy.Direction == LookingDirection.Left
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
			Vector2.left * (float)_enemy.Direction;
    }
}