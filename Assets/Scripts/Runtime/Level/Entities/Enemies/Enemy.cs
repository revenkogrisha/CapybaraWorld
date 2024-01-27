using Core.Common;
using Core.Other;
using UnityEngine;

namespace Core.Level
{
    public class Enemy : Entity
	{
        [SerializeField] private bool _isImmortal = false;
        
        public bool TryPerformDeath()
        {
            if (_isImmortal == true)
                return false;
            
            gameObject.SelfDespawn();
            
            // Called here for simplicity. Should be in special class-manager or ~EnemyFeedbackHalder
            HapticHelper.VibrateMedium();

            return true;         
        }
    }
}