using Core.Player;
using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "UI Collection", menuName = "Collections/UI")]
    public class UICollection : ScriptableObject
    {
        [Header("Views")]
        [SerializeField] private MainMenuView _mainMenuPrefab;
        [SerializeField] private GameWinMenu _gameWinMenuPrefab;
        [SerializeField] private GameLostMenu _gameOverMenuPrefab;
        [SerializeField] private LoadingScreen _loadingScreenCanvasPrefab;
        [SerializeField] private HeroMenuView _heroMenuPrefab;
        [SerializeField] private SettingsMenuView _settingsMenuPrefab;

        [Header("Common")]
        [SerializeField] private DashRecoveryDisplay _dashRecoveryDisplay;
        [SerializeField] private AreaLabelContainer _areaLabelContainer;
        [SerializeField] private MainMenuRoot _mainMenuRootPrefab;

        public MainMenuView MainMenuPrefab => _mainMenuPrefab;
        public GameWinMenu GameWinMenuPrefab => _gameWinMenuPrefab;
        public GameLostMenu GameOverMenuPrefab => _gameOverMenuPrefab;
        public LoadingScreen LoadingScreenCanvasPrefab => _loadingScreenCanvasPrefab;

        public DashRecoveryDisplay DashRecoveryDisplay => _dashRecoveryDisplay;
        public AreaLabelContainer AreaLabelContainer => _areaLabelContainer;

        public HeroMenuView HeroMenuPrefab => _heroMenuPrefab;
        public SettingsMenuView SettingsMenuPrefab => _settingsMenuPrefab;
        public MainMenuRoot MainMenuRootPrefab => _mainMenuRootPrefab;

    }
}
