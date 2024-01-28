#if !UNITY_EDITOR && UNITY_ANDROID
using System;
using System.Collections;
using System.Threading;
using Core.Editor.Debugger;
using Cysharp.Threading.Tasks;
using Google.Play.AppUpdate;
using Google.Play.Common;
using UnityEngine;

namespace Core.Common.ThirdParty
{
    public class AppUpdateService : IAppUpdateService
    {
        private AppUpdateManager _manager;
        private AppUpdateInfo _lastInfo;

        public void Initialize() => 
            _manager = new();

        public async UniTask<UpdateAvailability> StartUpdateCheck(CancellationToken token)
        {
            string log = $"{nameof(AppUpdateService)}::{nameof(StartUpdateCheck)}";
            
            try
            {
                if (_manager == null)
                {
                    RDebug.Error($"{log}: manager is null");
                    return UpdateAvailability.Unknown;
                }

                await CheckForUpdateCoroutine().ToUniTask(cancellationToken: token);
            
                if (_lastInfo == null)
                {
                    RDebug.Error($"{log}: info is null");
                    return UpdateAvailability.Unknown;
                }

                return _lastInfo.UpdateAvailability;
            }
            catch (Exception ex)
            {
                RDebug.Error($"{log}: {ex.Message} \n{ex.StackTrace}");
                return UpdateAvailability.Unknown;
            }
        }

        public async UniTask Request(CancellationToken token)
        {
            string log = $"{nameof(AppUpdateService)}::{nameof(Request)}";
            
            try
            {
                if (_lastInfo == null)
                {
                    RDebug.Error($"{log}: info is null");
                    return;
                }
            
                if (_manager == null)
                {
                    RDebug.Error($"{log}: manager is null");
                    return;
                }

                await RequestCoroutine().ToUniTask(cancellationToken: token);
            }
            catch (Exception ex)
            {
                RDebug.Error($"{log}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        public void Clear()
        {
            _manager = null;
            _lastInfo = null;
        }

        private IEnumerator RequestCoroutine(bool clearOnEnd = true)
        {
            AppUpdateOptions appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
            AppUpdateRequest request = _manager.StartUpdate(_lastInfo, appUpdateOptions);
            yield return request;

            if (clearOnEnd == true)
                Clear();
        }

        private IEnumerator CheckForUpdateCoroutine()
        {
            string log = $"{nameof(AppUpdateService)}::{nameof(CheckForUpdateCoroutine)}";
            
            PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
                _manager.GetAppUpdateInfo();

            float operationStartTime = Time.time;

            yield return appUpdateInfoOperation;

            float operationDuration = Time.time - operationStartTime;
            RDebug.Info($"{log}: Update operation completed. Duration: {operationDuration}");

            if (appUpdateInfoOperation.IsSuccessful == true)
            {
                _lastInfo = appUpdateInfoOperation.GetResult();
                ValidateUnexpectedAvailability();
            }
            else
            {
                RDebug.Error($"{log}: async operation failed: {appUpdateInfoOperation.Error}");
            }
        }

        private void ValidateUnexpectedAvailability()
        {
            string log = $"{nameof(AppUpdateService)}::{nameof(ValidateUnexpectedAvailability)}";
            
            if (_lastInfo == null)
            {
                RDebug.Warning($"{log}: info is null");
                return;
            }
            
            switch (_lastInfo.UpdateAvailability)
            {
                case UpdateAvailability.Unknown:
                case UpdateAvailability.DeveloperTriggeredUpdateInProgress:
                    RDebug.Warning($"{log}: Validation result: {_lastInfo.UpdateAvailability}");
                    return;
            }
        }
    }
}
#endif