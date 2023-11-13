using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.UI
{
    public class AnimatedUI : MonoBehaviour, IAnimatedUI
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Settings")]
        [SerializeField] private float _revealSpeed = 0.05f;
        [SerializeField] private float _concealSpeed = 0.05f;

        public async UniTask Reveal()
        {
            CancellationToken token = destroyCancellationToken;
            _canvasGroup.alpha = 0f;

            bool canceled = false;
            while (canceled == false && _canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += _revealSpeed;

                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }
        }

        public async UniTask Conceal()
        {
            CancellationToken token = default;
            _canvasGroup.alpha = 1f;

            bool canceled = false;
            while (canceled == false && _canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= _concealSpeed;

                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }
        }
    }
}
