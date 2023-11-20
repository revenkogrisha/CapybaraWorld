using System;
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

        public async UniTask Reveal(CancellationToken token = default)
        {
            try
            {
                _canvasGroup.alpha = 0f;

                while (_canvasGroup.alpha < 1f)
                {
                    _canvasGroup.alpha += _revealSpeed;

                    await UniTask.NextFrame(token);
                }
            }
            catch
            {
                print("Exception handled in Animated UI");
            }
        }

        public async UniTask Conceal(CancellationToken token = default)
        {
            try
            {
                _canvasGroup.alpha = 1f;

                while (_canvasGroup.alpha > 0f)
                {
                    _canvasGroup.alpha -= _concealSpeed;

                    await UniTask.NextFrame(token);
                }
            }
            catch
            {
                print("Exception handled in Animated UI");
            }
        }
    }
}
