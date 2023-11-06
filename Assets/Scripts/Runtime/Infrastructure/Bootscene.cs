using Core.Common;
using Core.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootscene : MonoBehaviour
    {
        private UIProvider _uiProvider;

        [Inject]
        private void Construct(UIProvider uiProvider) =>
            _uiProvider = uiProvider;

        private void Start()
        {
            UniTask task = SceneLoader.LoadGameplay();
            task.Forget();

            _uiProvider.CreateLoadingScreen();
        }
    }
}
