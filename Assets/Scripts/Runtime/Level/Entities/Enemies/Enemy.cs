using Core.Other;

namespace Core.Level
{
    public class Enemy : Entity
	{
        public void PerformDeath() => 
			gameObject.SelfDespawn();
    }
}