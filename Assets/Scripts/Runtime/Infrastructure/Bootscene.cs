using Core.Common;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootscene : MonoBehaviour
    {
        private const int TargetFrameRate = 60;

        private LoadingProvider _loadingProvider;

        [Inject]
        private void Construct(LoadingProvider loadingProvider) =>
            _loadingProvider = loadingProvider;

        private async void Start()
        {
            SetupApplication();
            await _loadingProvider.LoadGameWithScreen();
        }

        private void SetupApplication() =>
            Application.targetFrameRate = TargetFrameRate;
    }
}
