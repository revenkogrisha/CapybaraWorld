using System;
using UnityEngine;

namespace Core.Saving
{
    public static class PlayerPrefsUtility
    {
        private const string LevelsWonKey = nameof(LevelsWonKey);
        private const string MusicVolumeKey = nameof(MusicVolumeKey);
        private const string SoundsVolumeKey = nameof(SoundsVolumeKey);
        private const string HasRequestedReviewKey = nameof(HasRequestedReviewKey);
        private const string HapticFeedbackEnabledKey = nameof(HapticFeedbackEnabledKey);

        private const int LevelsWonDefault = 0;
        private const int MusicVolumeDefault = 0;
        private const int SoundsVolumeDefault = 0;
        private const int HasRequestedReviewDefault = -1;
        private const int HapticFeedbackEnabledDefault = 1;
        
        public static int LevelsWon
        {
            get => PlayerPrefs.GetInt(LevelsWonKey, LevelsWonDefault);
            set => PlayerPrefs.SetInt(LevelsWonKey, value);
        }
        
        public static float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MusicVolumeKey, MusicVolumeDefault);
            set => PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }
        
        public static float SoundsVolume
        {
            get => PlayerPrefs.GetFloat(SoundsVolumeKey, SoundsVolumeDefault);
            set => PlayerPrefs.SetFloat(SoundsVolumeKey, value);
        }

        public static bool HasRequestedReview
        {
            get
            {
                int pref = PlayerPrefs.GetInt(HasRequestedReviewKey, HasRequestedReviewDefault);
                return pref != -1 && Convert.ToBoolean(pref);
            }
            set => PlayerPrefs.SetInt(HasRequestedReviewKey, Convert.ToInt32(value));
        }

        public static bool HapticFeedbackEnabled
        {
            get
            {
                int pref = PlayerPrefs.GetInt(HapticFeedbackEnabledKey,
                    HapticFeedbackEnabledDefault);

                return pref != -1 && Convert.ToBoolean(pref);
            }
            set => PlayerPrefs.SetInt(HapticFeedbackEnabledKey, Convert.ToInt32(value));
        }

        public static void ClearData() =>
            PlayerPrefs.DeleteAll();
    }
}