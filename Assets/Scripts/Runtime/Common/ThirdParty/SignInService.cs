#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using Core.Editor.Debugger;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using UnityEngine;

namespace Core.Common.ThirdParty
{
    public static class SignInService
    {
        public static bool IsAuthenticated => PlayGamesPlatform.Instance.IsAuthenticated();
        public static string UserId => PlayGamesPlatform.Instance.GetUserId();

        public static async UniTaskVoid Authenticate()
        {
            try
            {
                UniTaskCompletionSource completionSource = new();
    
                if (Application.internetReachability == NetworkReachability.NotReachable)
                    return;
    
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate(status =>
                {
                    if (status == GooglePlayGames.BasicApi.SignInStatus.Success)
                    {
                        RDebug.Info($"{nameof(SignInService)}: Authorization complete with result: {status}");
                    }
                    else 
                    {
                        RDebug.Warning($"{nameof(SignInService)}::{nameof(Authenticate)}: Authorization failed! Status: {status}");
                        PlayGamesPlatform.Instance.ManuallyAuthenticate(status => 
                        {
                            RDebug.Info($"{nameof(SignInService)}::{nameof(Authenticate)}: Manual auth status: {status}");
                        });
                    }
    
                    completionSource.TrySetResult();
                });
    
                await completionSource.Task;
            }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(SignInService)}::{nameof(Authenticate)}: {ex.Message} \n {ex.StackTrace}");
            }
        }
    }
}
#endif