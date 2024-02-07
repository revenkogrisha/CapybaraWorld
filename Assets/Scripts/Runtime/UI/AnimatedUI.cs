using System;
using System.Threading;
using Core.Editor.Debugger;
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

        public virtual async UniTask Reveal(
            CancellationToken token = default,
             bool enable = false)
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
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"UniTaskVoid exception: {nameof(AnimatedUI)}::{nameof(Reveal)}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        public virtual async UniTask Conceal(
            CancellationToken token = default,
            bool disable = false)
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
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(AnimatedUI)}::{nameof(Conceal)}: {ex.Message} \n{ex.StackTrace}");
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