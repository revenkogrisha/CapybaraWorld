using Core.Game;
using Core.Mediation;
using Core.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public class GameLostMenu : FinishMenu
    {
        [Header("Progress Bar")]
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Image _heroImage;

        protected IPlaythroughProgressHandler _playthrough;
        private HeroSkins _heroSkins;
        private IMediationService _mediationService;

        protected override void OnEnable()
        {
            _mediationService.ShowInterstitial();
            base.OnEnable();
        }

        private void Start()
        {
            SetProgressBarValue(_playthrough.Progress);
            _heroImage.sprite = _heroSkins.Current.Head;
        }

        [Inject]
        private void Construct(
            IPlaythroughProgressHandler playthrough,
            HeroSkins heroSkins,
            IMediationService mediationService)
        {
            _playthrough = playthrough;
            _heroSkins = heroSkins;
            _mediationService = mediationService;
        }

        private void SetProgressBarValue(float delta) => 
            _progressBar.value = delta;
    }
}
