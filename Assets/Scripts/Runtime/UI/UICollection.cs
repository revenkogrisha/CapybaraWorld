using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "UI Collection", menuName = "Collections/UI")]
    public class UICollection : ScriptableObject
    {
        [Header("Views")]
        [SerializeField] private MainMenu _mainMenuPrefab;
        [SerializeField] private GameOverMenu _gameOverMenuPrefab;
        [SerializeField] private LoadingScreen _loadingScreenCanvasPrefab;

        [Header("Common")]
        [SerializeField] private ScoreDisplay _scorePrefab;

        public MainMenu MainMenuPrefab => _mainMenuPrefab;
        public GameOverMenu GameOverMenuPrefab => _gameOverMenuPrefab;
        public LoadingScreen LoadingScreenCanvasPrefab => _loadingScreenCanvasPrefab;

        public ScoreDisplay ScorePrefab => _scorePrefab;
    }
}
