using Core.Common;
using Core.Other;

namespace Core.Level
{
    public class Enemy : Entity
	{
        public void PerformDeath()
        {
            gameObject.SelfDespawn();
            
            // Called here for simplicity. Should be in special class-manager or ~EnemyFeedbackHalder
            HapticHelper.VibrateMedium();
        }
    }
}