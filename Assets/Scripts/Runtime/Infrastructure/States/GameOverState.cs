using Core.Factories;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class GameOverState : State
    {
        private readonly UIProvider _uiProvider;
        private GameOverMenu _gameOverMenu;

        [Inject]
        public GameOverState(UIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public override void Enter()
        {
            _gameOverMenu = _uiProvider.CreateGameOverMenu();
        }

        public override void Exit()
        {
            Object.Destroy(_gameOverMenu.gameObject);
        }
    }
}
