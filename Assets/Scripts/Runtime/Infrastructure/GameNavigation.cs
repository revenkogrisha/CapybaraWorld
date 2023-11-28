using Core.Common;
using Zenject;

namespace Core.Infrastructure
{
    public class GameNavigation
    {
        private readonly IGameStateMachine _globalStateMachine;

        [Inject]
        public GameNavigation(IGameStateMachine globalStateMachine) =>
            _globalStateMachine = globalStateMachine;

        public void ToLoadingData() => 
            _globalStateMachine.ChangeState<DataInitializationState>();

        public void Generate<TNextState>() 
            where TNextState : State
        {
            TNextState nextState = _globalStateMachine.GetState<TNextState>();
            GenerationState generationState = _globalStateMachine
                .GetStateWithArg<GenerationState, State>(nextState);

            _globalStateMachine.ChangeState(generationState);
        }

        public void ToGameplay() =>
            _globalStateMachine.ChangeState<GameplayState>();

        public void ToWin() =>
            _globalStateMachine.ChangeState<GameWinState>();

        public void ToLost() =>
            _globalStateMachine.ChangeState<GameLostState>();
    }
}