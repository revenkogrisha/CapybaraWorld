using Core.Common;
using Core.Other;
using UnityEngine;
using Zenject;

namespace Core.Level
{
    public class Enemy : Entity
	{
        [SerializeField] private bool _isImmortal = false;
        
        private ParticlesHelper _particlesHelper;

        [Inject]
        private void BaseConstruct(ParticlesHelper particlesHelper) =>
            _particlesHelper = particlesHelper;
        
        public bool TryPerformDeath()
        {
            if (_isImmortal == true)
                return false;
            
            _particlesHelper
                .Spawn(ParticlesName.EnemyDeath, transform.position)
                .Forget();

            gameObject.SelfDestroy();
            
            // Called here for simplicity. Should be in special class-manager or ~EnemyFeedbackHalder
            HapticHelper.VibrateMedium();

            return true;         
        }
    }
}