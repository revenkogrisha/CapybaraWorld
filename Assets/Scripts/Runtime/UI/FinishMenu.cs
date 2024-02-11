using Core.Infrastructure;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class FinishMenu : AnimatedUI
    {
        private const float TitleTweenDuration = 0.5f;
        private const float ButtonShakeDelayByTitle = 0.5f;
        
        [Title("Buttons")]
        [SerializeField] private Transform _title;
        [SerializeField] private bool _tween = true;
        [SerializeField, ShowIf(nameof(_tween))] private float _finalScale = 1f;
        
        [Title("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        private GameNavigation _navigation;

        protected virtual void OnEnable()
        {
            _restartButton.OnClicked += RestartGame;
            _menuButton.OnClicked += ReturnToMenu;

            TweenElements();
        }

        protected virtual void OnDisable()
        {
            _restartButton.OnClicked -= RestartGame;
            _menuButton.OnClicked -= ReturnToMenu;
        }

        [Inject]
        private void BaseConstruct(GameNavigation navigation) =>
            _navigation = navigation;

        private void RestartGame() => 
            _navigation.Generate<GameplayState>();

        private void ReturnToMenu() => 
            _navigation.Generate<MainMenuState>();

        private void TweenElements()
        {
            if (_tween == true)
            {
                _title.localScale = Vector2.zero;
                DOTween.Sequence()
                    .Append(_title.DOScale(_finalScale, TitleTweenDuration))
                    .AppendInterval(ButtonShakeDelayByTitle)
                    .AppendCallback(() => _restartButton.Shake());
            }
        }
    }
}