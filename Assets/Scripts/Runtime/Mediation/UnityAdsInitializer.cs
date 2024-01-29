using Core.Editor.Debugger;
using UnityEngine.Advertisements;

namespace Core.Mediation.UnityAds
{
    public static class UnityAdsInitializer
    {   
        public static void Initialize(IUnityAdsInitializationListener listener)
        {
#if UNITY_ANDROID || UNITY_EDITOR
            string id = UnityAdsData.AndroidGameId;
#else
            string id = UnityAdsData.IOSGameId;
#endif

#if UNITY_EDITOR || REVENKO_DEVELOP
            bool testMode = true;
#else
            bool testMode = false;
#endif
            
            if (Advertisement.isInitialized == false && Advertisement.isSupported == true)
                Advertisement.Initialize(id, testMode, listener);
            else
                RDebug.Warning($"Cannot initialize UnityAds: initialized={Advertisement.isInitialized}, supported:{Advertisement.isSupported}");
        }
    }
}
