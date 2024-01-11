using Core.Editor.Debugger;
using UnityEngine;

namespace Core.Player
{
    public class AccentColorHandler : MonoBehaviour
    {
        private Color _accentColor = Color.white;

        public Color AccentColor
        {
            get => _accentColor;
            set
            {
                if (value.a < 1f)
                {
                    RDebug.Error($"{nameof(AccentColorHandler)}: Accent color cannot be transparent - alfa: {value.a}");
                    return;
                }

                _accentColor = value;
            }
        }
    }
}