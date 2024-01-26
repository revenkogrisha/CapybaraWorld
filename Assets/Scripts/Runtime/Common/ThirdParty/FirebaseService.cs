using System;
using System.Collections.Generic;
using System.Linq;
using Core.Editor.Debugger;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using UnityEngine;
using static Core.Common.ThirdParty.ParameterName;

namespace Core.Common.ThirdParty
{
    public static class FirebaseService
    {
        private const bool IsAnalyticsCollectionEnabled = true;
        private const bool IsCrashlyticsCollectionEnabled = true;
        private const bool IsReportUncaughtAsFatal = true;
        
        private static FirebaseApp _firebaseInstance = null;

        private static bool IsInitialized => _firebaseInstance != null;

        public static async UniTaskVoid Initialize()
        {
            try
            {
                _firebaseInstance = null;

                await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
                {
                    if (task.Result == DependencyStatus.Available) 
                    {
                        _firebaseInstance = FirebaseApp.DefaultInstance;

                        InitializeAnalytics();
                        InitializeCrashlytics();

                        RDebug.Info($"Firebase services were successfully initialized!");
                    } 
                    else 
                    {
                        RDebug.Error($"{nameof(FirebaseService)}::{nameof(Initialize)}: Could not resolve all Firebase dependencies: {task.Result}");
    
                        if (task.Exception != null)
                            RDebug.Error($"{nameof(FirebaseService)}::{nameof(Initialize)}: Exception while resolution: {task.Exception.Message} \n {task.Exception.StackTrace}");
    
                        return;
                    }
                });
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(FirebaseService)}::{nameof(Initialize)}: {ex.Message} \n {ex.StackTrace}");
                _firebaseInstance = null;
            }
        }

        public static void LogEvent(EventName name) => 
            LogEvent(name.ToString(), null);

        public static void LogEvent<T>(EventName name, IDictionary<ParameterName, T> parameters) 
            where T : IConvertible
        {
            Parameter[] parametersArray = new Parameter[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                KeyValuePair<ParameterName, T> pair = parameters.ElementAt(i);
                parametersArray[i] = new(pair.Key.ToString(), pair.Value.ToString());
            }
            
            LogEvent(name.ToString(), parametersArray);
        }

        private static void InitializeAnalytics()
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(IsAnalyticsCollectionEnabled);

            if (Social.localUser != null)
                FirebaseAnalytics.SetUserId(Social.localUser.id);
        }

        private static void InitializeCrashlytics()
        {
            Crashlytics.IsCrashlyticsCollectionEnabled = IsCrashlyticsCollectionEnabled;
            Crashlytics.ReportUncaughtExceptionsAsFatal = IsReportUncaughtAsFatal;

            if (Social.localUser != null)
                FirebaseAnalytics.SetUserId(Social.localUser.id);
        }

        private static void LogEvent(string name, IEnumerable<Parameter> parameters)
        {
            try
            {
                if (IsInitialized == false)
                {
                    RDebug.Warning($"{nameof(FirebaseService)}: Tried to log event before initialization!");
                    return;
                }
    
                Parameter[] commonParameters = GetCommonParameters();
    
                if (parameters == null)
                {
                    FirebaseAnalytics.LogEvent(name, commonParameters);
                    RDebug.Info($"Event {name} was successfully logged!");
                    return;
                }
    
                FirebaseAnalytics.LogEvent(name, commonParameters.Concat(parameters).ToArray());

                RDebug.Info($"Event {name} was successfully logged!");
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(FirebaseService)}::{nameof(LogEvent)}: {ex.Message} \n {ex.StackTrace}");
                _firebaseInstance = null;
            }
        }

        private static Parameter[] GetCommonParameters()
        {
#if REVENKO_DEVELOP || UNITY_EDITOR
            const string buildType = "DEV";
#else
            const string buildType = "RELEASE";
#endif

            return new[]
            {
                new Parameter(BuildType.ToString(), buildType),
                new Parameter(BuildVersion.ToString(), Application.version),
                new Parameter(BuildPlatform.ToString(), Application.platform.ToString())
            };
        }
    }
}