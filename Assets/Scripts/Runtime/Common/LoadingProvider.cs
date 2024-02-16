using Core.Factories;
using Core.Other;
using Core.UI;
using Cysharp.Threading.Tasks;
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

            Scene gameplayScene = await SceneLoader.LoadGameplay(LoadSceneMode.Additive);

            SceneManager.MoveGameObjectToScene(loadingScreen.gameObject, gameplayScene);

            await loadingScreen.Conceal();
            loadingScreen.gameObject.SelfDestroy();

            await SceneLoader.UnloadCurrentAsync();
        }
    }
}