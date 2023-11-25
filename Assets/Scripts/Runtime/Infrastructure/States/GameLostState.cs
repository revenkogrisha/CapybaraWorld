using Core.Common;
using Core.Factories;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class GameLostState : State
    {
        private readonly UIProvider _uiProvider;
        private GameLostMenu _gameOverMenu;

        [Inject]
        public GameLostState(UIProvider uiProvider) => 
            _uiProvider = uiProvider;

        public override void Enter() => 
            _gameOverMenu = _uiProvider.CreateGameOverMenu();

        public override void Exit() => 
            Object.Destroy(_gameOverMenu.gameObject);
    }
}
