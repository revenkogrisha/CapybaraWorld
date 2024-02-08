using Core.Player;
using Core.UI;
using UnityEngine;
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

        public MainMenuView CreateMainMenu(RectTransform parent = null)
        {
            if (parent == null)
                parent = _root.RectTransform;
            
            MainMenuView mainMenu = _diContainer
                .InstantiatePrefabForComponent<MainMenuView>(
                    _collection.MainMenuPrefab,
                    parent);
            
            return mainMenu;
        }
        
        public HeroMenuView CreateHeroMenu(RectTransform parent = null)
        {
            if (parent == null)
                parent = _root.RectTransform;
            
            HeroMenuView heroMenu = _diContainer
                .InstantiatePrefabForComponent<HeroMenuView>(
                    _collection.HeroMenuPrefab,
                    parent);
            
            return heroMenu;
        }
        
        public SettingsMenuView CreateSettingsMenu(RectTransform parent = null)
        {
            if (parent == null)
                parent = _root.RectTransform;
            
            SettingsMenuView settingsMenu = _diContainer
                .InstantiatePrefabForComponent<SettingsMenuView>(
                    _collection.SettingsMenuPrefab,
                    parent);
            
            return settingsMenu;
        }
        
        public MainMenuRoot CreateMainMenuRoot()
        {
            MainMenuRoot root = _diContainer
                .InstantiatePrefabForComponent<MainMenuRoot>(
                    _collection.MainMenuRootPrefab,
                    _root.RectTransform);
            
            return root;
        }

        public GameWinMenu CreateGameWinMenu()
        {
            GameWinMenu gameWinMenu = _diContainer
                .InstantiatePrefabForComponent<GameWinMenu>(
                    _collection.GameWinMenuPrefab,
                    _root.RectTransform);
            
            return gameWinMenu;
        }

        public GameLostMenu CreateGameLostMenu()
        {
            GameLostMenu gameOverMenu = _diContainer
                .InstantiatePrefabForComponent<GameLostMenu>(
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

        public DashRecoveryDisplay CreateDashRecoveryDisplay()
        {
            DashRecoveryDisplay display = _diContainer
                .InstantiatePrefabForComponent<DashRecoveryDisplay>(
                    _collection.DashRecoveryDisplay);

            return display;
        }

        public AreaLabelContainer CreateAreaLabelContainer()
        {
            AreaLabelContainer container = _diContainer
                .InstantiatePrefabForComponent<AreaLabelContainer>(
                    _collection.AreaLabelContainer,
                    _root.RectTransform);

            return container;
        }
    }
}