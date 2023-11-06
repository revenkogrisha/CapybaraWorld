using Core.Common;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootscene : MonoBehaviour
    {
        private LoadingProvider _loadingProvider;

        [Inject]
        private void Construct(LoadingProvider loadingProvider) =>
            _loadingProvider = loadingProvider;

        private async void Start() =>
            await _loadingProvider.LoadGameWithScreen();
    }
}
