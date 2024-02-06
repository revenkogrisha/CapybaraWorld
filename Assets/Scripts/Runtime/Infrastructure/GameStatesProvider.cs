using Zenject;

namespace Core.Infrastructure
{
    public class GameStatesProvider
    {
        private readonly DataInitializationState _dataInitializationState;
        private readonly GenerationState _generationState;
        private readonly MainMenuState _mainMenuState;
        private readonly GameplayState _gameplayState;
        private readonly GameWinState _gameWinState;
        private readonly GameLostState _gameLostState;

        public DataInitializationState DataInitializationState => _dataInitializationState;
        public GenerationState GenerationState => _generationState;
        public MainMenuState MainMenuState => _mainMenuState;
        public GameplayState GameplayState => _gameplayState;
        public GameWinState GameWinState => _gameWinState;
        public GameLostState GameLostState => _gameLostState;

        [Inject]
        public GameStatesProvider(
            DataInitializationState dataInitializationState,
            GenerationState generationState,
            MainMenuState mainMenuState,
            GameplayState gameplayState,
            GameWinState gameWinState,
            GameLostState gameOverState)
        {
            _dataInitializationState = dataInitializationState;
            _generationState = generationState;
            _mainMenuState = mainMenuState;
            _gameplayState = gameplayState;
            _gameWinState = gameWinState;
            _gameLostState = gameOverState;
        }
    }
}
