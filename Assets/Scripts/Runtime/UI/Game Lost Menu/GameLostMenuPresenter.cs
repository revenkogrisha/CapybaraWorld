using Core.Game;
using Core.Player;

namespace Core.UI
{
    public class GameLostMenuPresenter
    {
        private readonly HeroSkins _heroSkins;
        private readonly IPlaythroughProgressHandler _playthrough;
        private readonly GameLostMenuView _view;

        public GameLostMenuPresenter(
            HeroSkins heroSkins,
            IPlaythroughProgressHandler playthrough, 
            GameLostMenuView view)
        {
            _heroSkins = heroSkins;
            _playthrough = playthrough;
            _view = view;
        }

        public void OnViewReveal()
        {
            _view.SetProgressBarValue(_playthrough.ProgressDelta);
            _view.SetHeroHeadSkin(_heroSkins.Current.Head);
        }
    }
}