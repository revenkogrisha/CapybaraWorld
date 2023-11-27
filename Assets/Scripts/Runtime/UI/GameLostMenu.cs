using Core.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public class GameLostMenu : FinishMenu
    {
        [Space]
        [SerializeField] private Slider _progressBar;

        protected IPlaythroughProgressHandler _playthrough;

        private void Start() => 
            SetProgressBarValue(_playthrough.PlaythroughProgress);

        [Inject]
        private void Construct(IPlaythroughProgressHandler playthrough) =>        
            _playthrough = playthrough;

        private void SetProgressBarValue(float delta) => 
            _progressBar.value = delta;
    }
}
