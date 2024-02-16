using System.Threading;
using Core.Mediation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public class GameLostMenuView : FinishMenu
    {
        [Header("Progress Bar")]
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Image _heroImage;

        private IMediationService _mediationService;
        private GameLostMenuPresenter _presenter;

        [Inject]
        private void Construct(IMediationService mediationService) => 
            _mediationService = mediationService;

        public override UniTask Reveal(CancellationToken token = default, bool enable = false)
        {
            _presenter.OnViewReveal();
            _mediationService.ShowInterstitial();
            
            return base.Reveal(token, enable);
        }

        public void Initialize(GameLostMenuPresenter presenter) =>
            _presenter = presenter;

        public void SetProgressBarValue(float delta) => 
            _progressBar.value = delta;

        public void SetHeroHeadSkin(Sprite head) => 
            _heroImage.sprite = head;
    }
}