#if !UNITY_STANDALONE
using CandyCoded.HapticFeedback;
using Core.Editor.Debugger;

namespace Core.Common
{
    public static class HapticHelper
    {
        public static bool Enabled { get; set; } = true;

        public static void VibrateLight()
        {
            if (Enabled == true)
            {
                HapticFeedback.LightFeedback();
                RDebug.Log($"{nameof(HapticHelper)}::{nameof(VibrateLight)}");
            }
        }

        public static void VibrateMedium()
        {
            if (Enabled == true)
            {
                HapticFeedback.MediumFeedback();
                RDebug.Log($"{nameof(HapticHelper)}::{nameof(VibrateMedium)}");
            }
        }

        public static void VibrateHeavy()
        {
            if (Enabled == true)
            {
                HapticFeedback.HeavyFeedback();
                RDebug.Log($"{nameof(HapticHelper)}::{nameof(VibrateHeavy)}");
            }
        }
    }
}
#endif