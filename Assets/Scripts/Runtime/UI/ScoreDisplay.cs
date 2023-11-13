using Core.Game;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreTMP;

        private Score _score;
        private CompositeDisposable _disposable = new();

        #region MonoBehaviour

        private void Awake() =>
            RegisterScore(_score);

        private void OnDisable() =>
            _disposable.Clear();

        #endregion

        [Inject]
        private void Construct(Score score) =>
            _score = score;

        private void RegisterScore(Score score)
        {
            score.PlaythroughScore
                .Subscribe(value => Display(value))
                .AddTo(_disposable);
        }

        private void Display(int value) =>
            _scoreTMP.SetText(value.ToString());
    }
}