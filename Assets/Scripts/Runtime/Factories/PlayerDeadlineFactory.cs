using Core.Level;
using Core.Other;
using Core.Player;
using UnityEngine;

namespace Core.Factories
{
    public class PlayerDeadlineFactory : IFactory<FollowerObject>
    {
        private readonly DeadlyForPlayerObject _playerDeadlinePrefab;
        private readonly Vector2 _playerDeadlinePosition;

        public PlayerDeadlineFactory(PlayerAssets assets)
        {
            _playerDeadlinePrefab = assets.PlayerDeadlinePrefab;
            _playerDeadlinePosition = assets.PlayerDeadlinePosition;
        }

        public FollowerObject Create()
        {
            DeadlyForPlayerObject deadlineObject = Object.Instantiate(_playerDeadlinePrefab);
            deadlineObject.SetPosition(_playerDeadlinePosition);
            FollowerObject playerDeadline = new(deadlineObject.transform);

            return playerDeadline;
        }
    }
}
