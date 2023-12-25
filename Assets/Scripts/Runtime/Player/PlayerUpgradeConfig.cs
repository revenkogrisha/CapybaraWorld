using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(menuName = "Configs/Player Upgrade Config", fileName = "Player Upgrade Config")]
    public class PlayerUpgradeConfig : ScriptableObject
    {
        [Header("Dash Cooldown")]
        [SerializeField, Min(1f)] private float _dashCooldownStep = 0.1f;
        [SerializeField, Min(1f)] private float _dashCooldownMax = 2f;
        
        [Header("Dash Speed")]
        [SerializeField, Min(1f)] private float _dashSpeedStep = 0.05f;
        [SerializeField, Min(1f)] private float _dashSpeedMax = 1.5f;

        public float DashCooldownStep => _dashCooldownStep;
        public float DashCooldownMax => _dashCooldownMax;

        public float DashSpeedStep => _dashSpeedStep;
        public float DashSpeedMax => _dashSpeedMax;
    }
}