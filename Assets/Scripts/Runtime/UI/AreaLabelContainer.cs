using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class AreaLabelContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _labelTMP;
        [SerializeField] private float _fadeDuration = 0.3f;

        private void Awake() => 
            gameObject.SetActive(false);

        public void SetLabel(string label) =>
            _labelTMP.SetText(label);

        public void TweenDisplay(float duration)
        {
            gameObject.SetActive(true);
            transform.localScale = Vector2.zero;

            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.one, _fadeDuration))
                .AppendInterval(duration)
                .Append(transform.DOScale(Vector2.zero, _fadeDuration))
                .AppendCallback(() => gameObject.SetActive(false));
        }
    }
}
