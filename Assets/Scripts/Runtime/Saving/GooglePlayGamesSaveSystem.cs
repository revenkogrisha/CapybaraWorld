#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using Core.Common.ThirdParty;
using Core.Editor.Debugger;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Zenject;

namespace Core.Saving
{
    public class GooglePlayGamesSaveSystem : ICloudSaveSystem
    {
        private readonly ISaveSystem _saveSystem;

        public bool IsAvailable => SignInService.IsAuthenticated;

        [Inject]
        public GooglePlayGamesSaveSystem(ISaveSystem saveSystem) =>
            _saveSystem = saveSystem;

        public void Save(SaveData data)
        {
            if (IsAvailable == false)
            {
                RDebug.Log("Not authenticated!");
                return;
            }
            
            OpenRemoteWithAction((status, game) => SaveInternal(status, game, data));
        }

        public void LoadToLocal()
        {
            if (IsAvailable == false)
            {
                RDebug.Warning("Not authenticated!");
                return;
            }
            
            OpenRemoteWithAction(LoadInternal);
        }

        private void OpenRemoteWithAction(Action<SavedGameRequestStatus, ISavedGameMetadata> action)
        {
            PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
                ISaveService.SaveFileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                action);
        }

        private void SaveInternal(SavedGameRequestStatus status, ISavedGameMetadata game, SaveData data) 
        {
            if (status == SavedGameRequestStatus.Success)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonSaveSystem.Serialize(data));

                SavedGameMetadataUpdate.Builder builder = new();
                SavedGameMetadataUpdate updatedMetadata = builder.Build();
                
                PlayGamesPlatform.Instance.SavedGame
                    .CommitUpdate(game, updatedMetadata, bytes, OnDataSaved);
            }
            else 
            {
                RDebug.Warning("Error opening game: " + status);
            }
        }

        private void LoadInternal(SavedGameRequestStatus status, ISavedGameMetadata game) 
        {
            if (status == SavedGameRequestStatus.Success)
                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, OnDataLoaded);
            else 
                RDebug.Warning("Error opening game: " + status);
        }

        private void OnDataSaved(SavedGameRequestStatus status, ISavedGameMetadata game) 
        {
            if (status == SavedGameRequestStatus.Success)
                RDebug.Log("Game " + game.Description + " written");
            else 
                RDebug.Warning("Error saving game: " + status);
        }

        private void OnDataLoaded(SavedGameRequestStatus status, byte[] data) 
        {
            if (status != SavedGameRequestStatus.Success)
            {
                RDebug.Warning("Error reading game: " + status);
                return;
            }

            RDebug.Log("Cloud data successfully loaded");

            if (data == null)
            {
                RDebug.Log("No data saved to the cloud yet...");
                return;
            }

            string stringData = Encoding.UTF8.GetString(data);
            if (string.IsNullOrEmpty(stringData) == false)
                File.WriteAllText(JsonSaveSystem.FilePath, stringData ,Encoding.UTF8);
            else
                RDebug.Warning("Clud data is empty...");
        }

        // -------------------- ### Extra UI for testing ### -------------------- 

        //call this with Unity button to view all saves on GPG. Bug: Can't close GPG window for some reason without restarting.
        public void showUI() {
            // user is ILocalUser from Social.LocalUser - will work when authenticated
            ShowSaveSystemUI(Social.localUser, (SelectUIStatus status, ISavedGameMetadata game) => {
                // do whatever you need with save bundle
            });
        }
        //displays savefiles in the cloud. This will only include one savefile if the m_saveName hasn't been changed
        private void ShowSaveSystemUI(ILocalUser user, Action<SelectUIStatus, ISavedGameMetadata> callback) {
            uint maxNumToDisplay = 3;
            bool allowCreateNew = true;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            if (savedGameClient != null) {
                savedGameClient.ShowSelectSavedGameUI(user.userName + "\u0027s saves",
                    maxNumToDisplay,
                    allowCreateNew,
                    allowDelete,
                    (SelectUIStatus status, ISavedGameMetadata saveGame) => {
                        // some error occured, just show window again
                        if (status != SelectUIStatus.SavedGameSelected) {
                            ShowSaveSystemUI(user, callback);
                            return;
                        }

                        callback?.Invoke(status, saveGame);
                    });
            } else {
                // this is usually due to incorrect APP ID
                RDebug.Error("Save Game client is null...");
            }
        }

    }
}
#endif