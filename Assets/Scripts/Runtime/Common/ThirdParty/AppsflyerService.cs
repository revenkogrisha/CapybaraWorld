using System;
using System.Collections.Generic;
using AppsFlyerSDK;
using Core.Editor.Debugger;

namespace Core.Common.ThirdParty
{
    public static class AppsflyerService
    {
        private const string DevKey = "sHyNHSKnMYYR4Zf5kAh3sP";
        private const bool EnableTCFDataCollection = true;
        
        public static void InitializeAndStart()
        {
            if (string.IsNullOrEmpty(DevKey) == true)
                return;

            try
            {
                // null because it's not for iOS (stated in method's summury)
                AppsFlyer.initSDK(DevKey, null);
    
                RDebug.Log($"{nameof(AppsflyerService)}: Init call complete!");
    
#if REVENKO_DEVELOP
                bool isDebug = true;
#else
                bool isDebug = false;
#endif
                AppsFlyer.enableTCFDataCollection(EnableTCFDataCollection);

                AppsFlyer.setIsDebug(isDebug);

#if UNITY_ANDROID && !UNITY_EDITOR
                string userId = SignInService.UserId;
                if (string.IsNullOrEmpty(userId) == false)
                    AppsFlyer.setCustomerUserId();
#endif

                AppsFlyer.startSDK();
                RDebug.Log($"{nameof(AppsflyerService)}::{nameof(InitializeAndStart)} SDK start complete!");
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(AppsflyerService)}: Initialization failed: \n{ex.Message} \n{ex.StackTrace}");
            }
        }

        public static void LogEvent(EventName eventName, Dictionary<string, string> parameters) =>
            LogEvent(eventName.ToString(), parameters);

        public static void LogEvent(string name, Dictionary<string, string> parameters) =>
            AppsFlyer.sendEvent(name, parameters);
    }
}
