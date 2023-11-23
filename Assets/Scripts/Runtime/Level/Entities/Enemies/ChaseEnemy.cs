using UnityEngine;

namespace Core.Level
{
    public class ChaseEnemy : SearchEnemyBase
	{
		[SerializeField] private ChaseEnemyPreset _preset;

		public ChaseEnemyPreset Preset => _preset;
		
        protected override void InitializeStates()
        {
			StateOnTrigger = new EnemyChaseState(this);
			StateMachine.AddState<EnemyChaseState>(StateOnTrigger);

            base.InitializeStates();
        }
    }
}