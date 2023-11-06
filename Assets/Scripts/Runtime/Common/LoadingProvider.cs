using Core.Factories;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.Common
{
    public class LoadingProvider
    {
        private readonly UIProvider _uiProvider;

        [Inject]
        public LoadingProvider(UIProvider uiProvider) =>
            _uiProvider = uiProvider;

        public async UniTask LoadGameWithScreen()
        {
            LoadingScreen loadingScreen = _uiProvider.CreateLoadingScreenCanvas();
            //await loadingScreen.RevealAsync();

            Scene gameplayScene = await SceneLoader.LoadGameplay(LoadSceneMode.Additive);

            SceneManager.MoveGameObjectToScene(loadingScreen.gameObject, gameplayScene);
            await loadingScreen.ConcealAsync();

            await SceneLoader.UnloadCurrentAsync();

        }
    }
}
