using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private FocusCamera _focusCameraReference;

        public override void InstallBindings()
        {
            BindPlayerCamera();
        }

        private void BindPlayerCamera()
        {
            Container
                .Bind<IPlayerCamera>()
                .To<FocusCamera>()
                .FromInstance(_focusCameraReference)
                .AsSingle()
                .Lazy();
        }
    }
}
