using System.Collections.Generic;
using Firebase.Analytics;

namespace Core.Common.ThirdParty
{
    /// <summary>
    /// A static class, which incorporates different third-party analytics services with the same logic
    /// </summary>
    public static class AnalyticsHandler
    {
        private const bool IsFirebaseLoggingEnabled = true;
        private const bool IsAppsflyerLoggingEnabled = true;

        public static void LogEvent(EventName name, Dictionary<string, string> parameters)
        {
            if (IsFirebaseLoggingEnabled == true)
                LogFirebaseEvent(name, parameters);

            if (IsAppsflyerLoggingEnabled == true)
                LogAppsflyerEvent(name, parameters);
        }

        private static void LogFirebaseEvent(EventName name, Dictionary<string, string> parameters)
        {
            var firebaseParameters = new Parameter[parameters.Count];

            int i = 0;
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                // As it's important for Firebase - which type we are logging with
                if (int.TryParse(pair.Value, out int intValue) == true)
                    firebaseParameters[i] = new(pair.Key, intValue);
                else
                    firebaseParameters[i] = new(pair.Key, pair.Value);
                    
                i++;
            }
            
            FirebaseService.LogEvent(name, firebaseParameters);
        }

        private static void LogAppsflyerEvent(EventName name, Dictionary<string, string> parameters)
        {
            AppsflyerService.LogEvent(name, parameters);
        }
    }
}
