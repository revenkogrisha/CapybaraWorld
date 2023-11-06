using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Common
{
    public static class SceneLoader
    {
        private const int BootsceneIndex = 0;
        private const int GameplayIndex = 1;

        public static async UniTask UnloadCurrentAsync()
        {
            Scene current = SceneManager.GetActiveScene();
            if (current != null)
                await SceneManager.UnloadSceneAsync(current);
        }

        public static async UniTask LoadBootscene(
            LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken token = default)
        {
            await SceneManager
                .LoadSceneAsync(BootsceneIndex, mode)
                .WithCancellation(token)
                .SuppressCancellationThrow();
        }

        public static async UniTask<Scene> LoadGameplay(
            LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken token = default)
        {
            int index = GameplayIndex;
            await SceneManager
                .LoadSceneAsync(index, mode)
                .WithCancellation(token)
                .SuppressCancellationThrow();
            
            return SceneManager.GetSceneByBuildIndex(index);
        }
    }
}