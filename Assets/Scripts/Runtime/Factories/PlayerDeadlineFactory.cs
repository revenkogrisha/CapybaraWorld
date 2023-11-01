using Core.Level;
using Core.Player;
using UnityEngine;

namespace Core.Factories
{
    public class PlayerDeadlineFactory : IFactory<FollowerObject>
    {
        private readonly FollowerObject _playerDeadlinePrefab;
        private readonly Vector2 _playerDeadlinePosition;

        public PlayerDeadlineFactory(PlayerConfig playerConfig)
        {
            _playerDeadlinePrefab = playerConfig.PlayerDeadlinePrefab;
            _playerDeadlinePosition = playerConfig.PlayerDeadlinePosition;
        }

        public FollowerObject Create(Transform heroTransform)
        {
            FollowerObject playerDeadline = Create();
            playerDeadline.Initialize(heroTransform);

            return playerDeadline;
        }

        public FollowerObject Create()
        {
            FollowerObject playerDeadline = Object.Instantiate(_playerDeadlinePrefab);
            playerDeadline.transform.position = _playerDeadlinePosition;

            return playerDeadline;
        }
    }
}
