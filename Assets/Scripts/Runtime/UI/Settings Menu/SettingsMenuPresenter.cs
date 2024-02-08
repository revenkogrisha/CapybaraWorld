namespace Core.UI
{
    public class SettingsMenuPresenter
    {
        private readonly SettingsMenu _model;
        private readonly SettingsMenuView _view;

        public SettingsMenuPresenter(SettingsMenu model, SettingsMenuView view)
        {
            _model = model;
            _view = view;
        }

        public void OnViewReveal()
        {
            _view.SetMusicButton(_model.IsMusicOn);
            _view.SetSoundsButton(_model.AreSoundsOn);
        }

        public void OnToggleMusic() => 
            _view.SetMusicButton(_model.ToggleMusic());

        public void OnToggleSounds() => 
            _view.SetSoundsButton(_model.ToggleSounds());

        public void OnLoadProgress() => 
            _model.TryLoadProgressOrSignIn();
    }
}