using System;

namespace Core.Player
{
    public class UpgradableStat
    {
        private float _multiplier = 1f;
        private readonly float _upgradeStep;
        private readonly float _maxMultiplier;

        public float Multiplier => _multiplier;

        public UpgradableStat(float upgradeStep, float maxMultiplier)
        {
            _upgradeStep = upgradeStep;
            _maxMultiplier = maxMultiplier;
        }

        public UpgradableStat(float upgradeStep, float maxMultiplier, float startMultiplier) : this(upgradeStep, maxMultiplier)
        {
            if (startMultiplier >= maxMultiplier)
                throw new ArgumentException("Max multiplier cannot be bigger than start multiplier!");
            
            _multiplier = startMultiplier;
        }

        public void Upgrade()
        {
            float upgraded = _multiplier + _upgradeStep;
            if (upgraded > _maxMultiplier)
            {
                _multiplier = _maxMultiplier;
                return;
            }
                
            _multiplier = upgraded;
        }
    }
}