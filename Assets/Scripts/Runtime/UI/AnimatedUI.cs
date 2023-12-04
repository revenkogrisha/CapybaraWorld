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

        public async UniTask Reveal(CancellationToken token = default, bool enable = false)
        {
            try
            {
                if (enable == true)
                    gameObject.SetActive(true);
                
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

        public async UniTask Conceal(CancellationToken token = default, bool disable = false)
        {
            try
            {
                _canvasGroup.alpha = 1f;

                while (_canvasGroup.alpha > 0f)
                {
                    _canvasGroup.alpha -= _concealSpeed;

                    await UniTask.NextFrame(token);
                }
                
                if (disable == true)
                    gameObject.SetActive(false);

            }
            catch
            {
                print("Exception handled in Animated UI");
            }
        }

        public void InstantReveal(bool enable = false)
        {
            if (enable == true)
                gameObject.SetActive(true);
                
            _canvasGroup.alpha = 1f;
        }

        public void InstantConceal(bool disable = false)
        {
            if (disable == true)
                gameObject.SetActive(false);
                
            _canvasGroup.alpha = 0f;
        }

    }
}