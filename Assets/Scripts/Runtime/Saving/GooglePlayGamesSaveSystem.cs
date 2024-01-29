#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using Core.Common.ThirdParty;
using Core.Editor.Debugger;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

namespace Core.Saving
{
    public class GooglePlayGamesSaveSystem : ICloudSaveSystem
    {
        public bool IsAvailable => SignInService.IsAuthenticated;

        public void Save(SaveData data)
        {
            if (IsAvailable == false)
            {
                RDebug.Log($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(Save)}: Not authenticated!");
                return;
            }
            
            OpenRemoteWithAction((status, game) => SaveInternal(status, game, data));
        }

        public void LoadToLocal()
        {
            if (IsAvailable == false)
            {
                RDebug.Warning($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(LoadToLocal)}: Not authenticated!");
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
                RDebug.Warning($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(SaveInternal)}: Error opening game: {status}" );
            }
        }

        private void LoadInternal(SavedGameRequestStatus status, ISavedGameMetadata game) 
        {
            if (status == SavedGameRequestStatus.Success)
                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, OnDataLoaded);
            else 
                RDebug.Warning($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(LoadInternal)}: Error opening game: {status}");
        }

        private void OnDataSaved(SavedGameRequestStatus status, ISavedGameMetadata game) 
        {
            if (status == SavedGameRequestStatus.Success)
                RDebug.Log($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(OnDataSaved)}: Game {game.Description} written");
            else 
                RDebug.Warning($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(OnDataSaved)}: Error saving game: {status}");
        }

        private void OnDataLoaded(SavedGameRequestStatus status, byte[] data) 
        {
            if (status != SavedGameRequestStatus.Success)
            {
                RDebug.Warning($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(OnDataLoaded)}: Error reading game: " + status);
                return;
            }

            RDebug.Log($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(OnDataLoaded)}: Cloud data successfully loaded");

            if (data == null)
            {
                RDebug.Log($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(OnDataLoaded)}: No data saved to the cloud yet...");
                return;
            }

            string stringData = Encoding.UTF8.GetString(data);
            if (string.IsNullOrEmpty(stringData) == false)
                File.WriteAllText(JsonSaveSystem.FilePath, stringData ,Encoding.UTF8);
            else
                RDebug.Warning($"{nameof(GooglePlayGamesSaveSystem)}::{nameof(OnDataLoaded)}: Cloud data is empty...");
        }

    }
}
#endif