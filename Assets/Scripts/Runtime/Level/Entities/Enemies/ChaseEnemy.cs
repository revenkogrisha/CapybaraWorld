using UnityEngine;

namespace Core.Level
{
    public class ChaseEnemy : SearchEnemyBase
	{
		[SerializeField] private ChaseEnemyPreset _chasePreset;

		public ChaseEnemyPreset Preset => _chasePreset;
		
        protected override void InitializeStates()
        {
			StateOnTrigger = new EnemyChaseState(this);
			StateMachine.AddState<EnemyChaseState>(StateOnTrigger);

            base.InitializeStates();
        }
    }
}