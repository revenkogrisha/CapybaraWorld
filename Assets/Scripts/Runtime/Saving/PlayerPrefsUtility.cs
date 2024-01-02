using System;
using UnityEngine;

namespace Core.Saving
{
    public static class PlayerPrefsUtility
    {
        private const string LevelsWonKey = nameof(LevelsWonKey);
        private const int LevelsWonDefault = 0;

        private const string HasRequestedReviewKey = nameof(HasRequestedReviewKey);
        
        public static int LevelsWon
        {
            get => PlayerPrefs.GetInt(LevelsWonKey, LevelsWonDefault);
            set => PlayerPrefs.SetInt(LevelsWonKey, value);
        }

        public static bool HasRequestedReview
        {
            get
            {
                int pref = PlayerPrefs.GetInt(HasRequestedReviewKey, -1);
                return pref != -1 && Convert.ToBoolean(pref);
            }
            set => PlayerPrefs.SetInt(HasRequestedReviewKey, Convert.ToInt32(value));
        }
    }
}