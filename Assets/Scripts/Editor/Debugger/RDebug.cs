#if UNITY_ANDROID && !UNITY_EDITOR
using Firebase.Crashlytics;
#endif
using Debug = UnityEngine.Debug;

namespace Core.Editor.Debugger
{
    public static class RDebug
    {
        private const string LogFormat = "{0}";
        private const string InfoFormat = "<color=cyan><b>Info:</b></color> {0}";
        private const string WarningFormat = "<color=yellow><b>Warning:</b></color> {0}";
        private const string ErrorFormat = "<color=red><b>Error:</b></color> {0}";

        public static void Log(object message) => 
            Debug.Log(string.Format(LogFormat, message));

        public static void Info(object message) => 
            Debug.Log(string.Format(InfoFormat, message));

        public static void Warning(object message, bool sendAsWarning = false)
        {
            string log = string.Format(WarningFormat, message); 
            if (sendAsWarning == true)
                Debug.LogWarning(log);
            else
                Debug.Log(log);
        }

        public static void Error(object message, bool sendAsError = false)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Crashlytics.Log(message.ToString());
            return;
#endif

            string log = string.Format(ErrorFormat, message);
            if (sendAsError == true)
                Debug.LogError(log);
            else
                Debug.Log(log);
        }
    }
}