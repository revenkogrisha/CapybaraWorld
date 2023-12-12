using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class FakeLoadingBar : MonoBehaviour
    {
        private const string CounterTemplateText = "{0}%";

        [SerializeField] private Slider _slider;
        [SerializeField] private bool _displayPercentage = true;
        [SerializeField] private TMP_Text _counter;

        [Space]
        [SerializeField, Range(0f, 0.5f)] private float _loadingSpeed = 0.01f;

        private void Start() =>
            Imitate().Forget();

        private async UniTaskVoid Imitate()
        {
            CancellationToken token = destroyCancellationToken;
            while (_slider.value < _slider.maxValue)
            {
                _slider.value += _loadingSpeed;
                if (_displayPercentage == true)
                    DisplayPercentage();

                await UniTask.NextFrame(token);
            }
        }

        private void DisplayPercentage() =>
            _counter.text = string.Format(CounterTemplateText, _slider.value * 100f);
    }
}