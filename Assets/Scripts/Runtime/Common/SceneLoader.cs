using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Common
{
    public static class SceneLoader
    {
        private const int BootsceneIndex = 0;
        private const int GameplayIndex = 1;

        public static async UniTask LoadBootscene(
            LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken token = default)
        {
            await SceneManager
                .LoadSceneAsync(BootsceneIndex, mode)
                .WithCancellation(token)
                .SuppressCancellationThrow();
        }

        public static async UniTask LoadGameplay(
            LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken token = default)
        {
            await SceneManager
                .LoadSceneAsync(GameplayIndex, mode)
                .WithCancellation(token)
                .SuppressCancellationThrow();
        }
    }
}