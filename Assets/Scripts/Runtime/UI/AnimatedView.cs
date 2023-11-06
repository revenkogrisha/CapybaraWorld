using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.UI
{
    public class AnimatedView : MonoBehaviour, IAnimatedView
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Settings")]
        [SerializeField] private float _revealSpeed = 0.05f;
        [SerializeField] private float _concealSpeed = 0.05f;

        public async UniTask RevealAsync()
        {
            CancellationToken token = destroyCancellationToken;

            _canvasGroup.alpha = 0f;
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += _revealSpeed;

                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }

        public async UniTask ConcealAsync()
        {
            CancellationToken token = default;

            _canvasGroup.alpha = 1f;
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= _concealSpeed;

                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }
    }
}
