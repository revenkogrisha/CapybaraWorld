using Core.Player;
using Core.UI;
using Zenject;

namespace Core.Factories
{
    public class UIProvider
    {
        private readonly UICollection _collection;
        private readonly IUIRoot _root;
        private readonly DiContainer _diContainer;

        [Inject]
        public UIProvider(UICollection collection, IUIRoot uiRoot, DiContainer diContainer)
        {
            _collection = collection;
            _root = uiRoot;
            _diContainer = diContainer;
        }

        public MainMenu CreateMainMenu()
        {
            MainMenu mainMenu = _diContainer
                .InstantiatePrefabForComponent<MainMenu>(
                    _collection.MainMenuPrefab,
                     _root.RectTransform);
            
            return mainMenu;
        }

        public GameOverMenu CreateGameOverMenu()
        {
            GameOverMenu gameOverMenu = _diContainer
                .InstantiatePrefabForComponent<GameOverMenu>(
                    _collection.GameOverMenuPrefab,
                     _root.RectTransform);
            
            return gameOverMenu;
        }

        public LoadingScreen CreateLoadingScreenCanvas()
        {
            LoadingScreen loadingScreen = _diContainer
                .InstantiatePrefabForComponent<LoadingScreen>(
                    _collection.LoadingScreenCanvasPrefab);
            
            return loadingScreen;
        }

        public ScoreDisplay CreateScoreDisplay()
        {
            ScoreDisplay scoreText = _diContainer
                .InstantiatePrefabForComponent<ScoreDisplay>(
                    _collection.ScorePrefab,
                    _root.RectTransform);

            return scoreText;
        }

        public DashRecoveryDisplay CreateDashRecoveryDisplay()
        {
            DashRecoveryDisplay display = _diContainer
                .InstantiatePrefabForComponent<DashRecoveryDisplay>(
                    _collection.DashRecoveryDisplay);

            return display;
        }
    }
}