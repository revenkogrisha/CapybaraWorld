using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private IGlobalStateMachine _stateMachine;

        private void Awake()
        {
            InitializeGlobalStates(_stateMachine);
            
            _stateMachine.ChangeState<GenerationState>();
        }

        [Inject]
        private void Construct(IGlobalStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void InitializeGlobalStates(IGlobalStateMachine stateMachine)
        {
            GenerationState generationState = new();

            stateMachine.AddState<GenerationState>(generationState);
            stateMachine.AddState<MainMenuState>(generationState);
            stateMachine.AddState<GameplayState>(generationState);
            stateMachine.AddState<GameOverState>(generationState);
        }
    }
}