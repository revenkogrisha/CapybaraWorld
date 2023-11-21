using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "New Chest", menuName = "Presets/Chest")]
    public class ChestPreset : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private int _coinsInside = 5;

        [Header("Behaviour")]
        [SerializeField] private float _minimumPushForce = 1f;
        [SerializeField] private float _maximumPushForce = 10f;

        public Coin CoinPrefab => _coinPrefab;
        public int CoinsInside => _coinsInside;
        public float MinimumPushForce => _minimumPushForce;
        public float MaximumPushForce => _maximumPushForce;
    }
}
