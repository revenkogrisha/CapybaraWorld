using Core.Player;
using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "UI Collection", menuName = "Collections/UI")]
    public class UICollection : ScriptableObject
    {
        [Header("Views")]
        [SerializeField] private MainMenu _mainMenuPrefab;
        [SerializeField] private GameWinMenu _gameWinMenuPrefab;
        [SerializeField] private GameLostMenu _gameOverMenuPrefab;
        [SerializeField] private LoadingScreen _loadingScreenCanvasPrefab;
        [SerializeField] private HeroMenu _heroMenuPrefab;

        [Header("Common")]
        [SerializeField] private DashRecoveryDisplay _dashRecoveryDisplay;
        [SerializeField] private AreaLabelContainer _areaLabelContainer;
        [SerializeField] private MainMenuRoot _mainMenuRootPrefab;

        public MainMenu MainMenuPrefab => _mainMenuPrefab;
        public GameWinMenu GameWinMenuPrefab => _gameWinMenuPrefab;
        public GameLostMenu GameOverMenuPrefab => _gameOverMenuPrefab;
        public LoadingScreen LoadingScreenCanvasPrefab => _loadingScreenCanvasPrefab;

        public DashRecoveryDisplay DashRecoveryDisplay => _dashRecoveryDisplay;
        public AreaLabelContainer AreaLabelContainer => _areaLabelContainer;

        public HeroMenu HeroMenuPrefab => _heroMenuPrefab;
        public MainMenuRoot MainMenuRootPrefab => _mainMenuRootPrefab;
    }
}
