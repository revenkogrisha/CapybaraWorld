using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private FocusCamera _focusCamera;

        public override void InstallBindings()
        {
            BindPlayerData();
            BindPlayerCamera();
        }

        private void BindPlayerData()
        {
            Container
                .Bind<PlayerData>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindPlayerCamera()
        {
            Container
                .Bind<IPlayerCamera>()
                .To<FocusCamera>()
                .FromInstance(_focusCamera)
                .AsSingle()
                .Lazy();
        }
    }
}