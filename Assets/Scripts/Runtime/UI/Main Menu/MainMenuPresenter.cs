namespace Core.UI
{
    public class MainMenuPresenter
    {
#if REVENKO_DEVELOP
        private readonly MainMenuDevHandler _dev;
#endif
        private readonly MainMenu _model;

        public MainMenuPresenter(
#if REVENKO_DEVELOP
            MainMenuDevHandler dev,
#endif
            MainMenu model)
        {
#if REVENKO_DEVELOP
            _dev = dev;
#endif
            _model = model;
        }

        public void OnStartGame() => 
            _model.StartGame();

        public string GetLocationName() => 
            _model.GetLocationName();

        public int GetLevelNumber() => 
            _model.GetLevelNumber();

#if REVENKO_DEVELOP
        public void OnDevUpdateLocation() => 
            _dev.UpdateLocation();

        public void OnDevCompleteLevel() =>
            _dev.CompleteLevel();

        public void OnDevShowAd() => 
            _dev.ShowAd();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR && REVENKO_DEVELOP
        public void OnDevSendNotification() =>
            _dev.SendNotification();
#endif
    }
}
