using UnityEngine;

namespace Core.Player
{
    public class HeroTrail : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _renderer;
        [SerializeField] private AccentColorHandler _accentColorHandler;

        private void Start()
        {
            if (_accentColorHandler != null)
                _renderer.startColor = _accentColorHandler.AccentColor;
        }
    }
}
