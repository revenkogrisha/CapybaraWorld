using Core.Common.ThirdParty;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private ThirdPartyInitializer _thirdParty;
        private IGameStateMachine _stateMachine;
        private GameStatesProvider _statesProvider;
        private GameNavigation _navigation;

        private void Awake()
        {
            _thirdParty.InitializeAll().Forget();
            
            AddGameStatesToMachine();
            _navigation.ToLoadingData();
        }

        [Inject]
        private void Construct(
            ThirdPartyInitializer thirdParty,
            IGameStateMachine stateMachine,
            GameStatesProvider statesProvider,
            GameNavigation navigation)
        {
            _thirdParty = thirdParty;
            _stateMachine = stateMachine;
            _statesProvider = statesProvider;
            _navigation = navigation;
        }

        private void AddGameStatesToMachine()
        {
            _stateMachine.AddState<DataInitializationState>(_statesProvider.DataInitializationState);
            _stateMachine.AddState<GenerationState>(_statesProvider.GenerationState);
            _stateMachine.AddState<MainMenuState>(_statesProvider.MainMenuState);
            _stateMachine.AddState<GameplayState>(_statesProvider.GameplayState);
            _stateMachine.AddState<GameWinState>(_statesProvider.GameWinState);
            _stateMachine.AddState<GameLostState>(_statesProvider.GameLostState);
        }
    }
}