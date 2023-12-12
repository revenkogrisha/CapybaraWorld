#if UNITY_EDITOR && UNITY_ANDROID
#define CUSTOM_DEBUG
#endif

using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Core.Editor
{
    public static class RDebug
    {
        private const string LogFormat = "{0}";
        private const string InfoFormat = "<color=cyan><b>Info:</b></color> {0}";
        private const string WarningFormat = "<color=yellow><b>Warning:</b></color> {0}";
        private const string ErrorFormat = "<color=red><b>Error:</b></color> {0}";

        [Conditional("CUSTOM_DEBUG")]
        public static void Log(string message) => 
            Debug.Log(string.Format(LogFormat, message));

        [Conditional("CUSTOM_DEBUG")]
        public static void Info(string message) => 
            Debug.Log(string.Format(InfoFormat, message));

        [Conditional("CUSTOM_DEBUG")]
        public static void Warning(string message, bool sendAsWarning = false)
        {
            string log = string.Format(WarningFormat, message); 
            if (sendAsWarning == true)
                Debug.LogWarning(log);
            else
                Debug.Log(log);
        }

        [Conditional("CUSTOM_DEBUG")]
        public static void Error(string message, bool sendAsError = false)
        {
            string log = string.Format(ErrorFormat, message);
            if (sendAsError == true)
                Debug.LogError(log);
            else
                Debug.Log(log);
        }
    }
}