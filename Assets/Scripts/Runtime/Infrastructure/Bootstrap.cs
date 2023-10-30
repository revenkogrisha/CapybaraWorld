using UnityEngine;
using Zenject;
using Core.Level;
using Core.Player;

namespace Core.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerTest _hero;

        private IGlobalStateMachine _stateMachine;
        private ILevelGenerator _levelGenerator;

        private void Awake()
        {
            InitializeGlobalStates(_stateMachine, _levelGenerator, _hero);

            _stateMachine.ChangeState<GenerationState>();
        }

        [Inject]
        private void Construct(
            IGlobalStateMachine stateMachine,
            ILevelGenerator levelGenerator)
        {
            _stateMachine = stateMachine;
            _levelGenerator = levelGenerator;
        }

        private void InitializeGlobalStates(
            IGlobalStateMachine stateMachine,
            ILevelGenerator levelGenerator,
            PlayerTest hero)
        {
            GenerationState generationState = new(levelGenerator, hero);
            MainMenuState mainMenuState = new();
            GameplayState gameplayState = new();
            GameOverState gameOverState = new();

            stateMachine.AddState<GenerationState>(generationState);
            stateMachine.AddState<MainMenuState>(mainMenuState);
            stateMachine.AddState<GameplayState>(gameplayState);
            stateMachine.AddState<GameOverState>(gameOverState);
        }
    }
}