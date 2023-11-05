using Core.Level;
using Core.Player;
using UnityEngine;

namespace Core.Factories
{
    public class PlayerDeadlineFactory : IFactory<FollowerObject>
    {
        private readonly DeadlyForPlayerObject _playerDeadlinePrefab;
        private readonly Vector2 _playerDeadlinePosition;

        public PlayerDeadlineFactory(PlayerConfig playerConfig)
        {
            _playerDeadlinePrefab = playerConfig.PlayerDeadlinePrefab;
            _playerDeadlinePosition = playerConfig.PlayerDeadlinePosition;
        }

        public FollowerObject Create()
        {
            DeadlyForPlayerObject deadlineObject = Object.Instantiate(_playerDeadlinePrefab);
            deadlineObject.transform.position = _playerDeadlinePosition;
            FollowerObject playerDeadline = new(deadlineObject.transform);

            return playerDeadline;
        }
    }
}
