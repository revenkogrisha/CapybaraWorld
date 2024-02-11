using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.UI
{
    public class SettingsMenuView : AnimatedUI
    {
        private const string MusicButtonFormat = "MUSIC:\n<color=#D978E9>{0}</color>";
        private const string SoundsButtonFormat = "SOUNDS:\n<color=#D978E9>{0}</color>";

        private const string On = "ON";
        private const string Off = "OFF";
        
        [Header("Buttons")]
        [SerializeField] private UIButton _musicToggleButton;
        [SerializeField] private UIButton _soundsToggleButton;
        [SerializeField] private UIButton _loadProgressButton;
        
        [Space]
        [SerializeField] private UIButton _backButton;

        private MainMenuRoot _root;
        private SettingsMenuPresenter _presenter;

        #region MonoBehaviour

        private void OnEnable()
        {
            _musicToggleButton.OnClicked += OnToggleMusic;
            _soundsToggleButton.OnClicked += OnToggleSounds;
            _loadProgressButton.OnClicked += OnLoadProgress;
            _backButton.OnClicked += ToMainMenu;
        }
        
        private void OnDisable()
        {
            _musicToggleButton.OnClicked -= OnToggleMusic;
            _soundsToggleButton.OnClicked -= OnToggleSounds;
            _loadProgressButton.OnClicked -= OnLoadProgress;
            _backButton.OnClicked -= ToMainMenu;
        }

        #endregion

        public override UniTask Reveal(CancellationToken token = default, bool enable = false)
        {
            _presenter.OnViewReveal();
            return base.Reveal(token, enable);
        }

        public void Initialize(MainMenuRoot root, SettingsMenuPresenter presenter)
        {
            _root = root;
            _presenter = presenter;
        }

        public void SetMusicButton(bool state)
        {
            _musicToggleButton.TMP.text = state == true
                ? string.Format(MusicButtonFormat, On)
                : string.Format(MusicButtonFormat, Off);
        }

        public void SetSoundsButton(bool state)
        {
            _soundsToggleButton.TMP.text = state == true
                ? string.Format(SoundsButtonFormat, On)
                : string.Format(SoundsButtonFormat, Off);
        }

        private void OnToggleMusic() => 
            _presenter.OnToggleMusic();

        private void OnToggleSounds() =>
            _presenter.OnToggleSounds();

        private void OnLoadProgress() =>
            _presenter.OnLoadProgress();

        private void ToMainMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowMainMenu();
        }
    }
}