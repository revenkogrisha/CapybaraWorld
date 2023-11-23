using Core.Other;
using UnityEngine;

namespace Core.Level
{
    public class Enemy : MonoBehaviour
	{
        public void PerformDeath() => 
			gameObject.SelfDespawn();
    }
}