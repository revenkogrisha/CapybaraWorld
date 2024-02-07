using Core.Factories;
using Core.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        private MainMenu _mainMenu;
        private HeroMenu _heroMenu;

        public RectTransform RectTransform => _rectTransform;

        public void InitializeMainMenu(MainMenu menu)
        {
            _mainMenu = menu;
            _mainMenu.Initialize(this);
        }

        public void InitializeHeroMenu(HeroMenu menu, HeroMenuPresenter presenter)
        {
            _heroMenu = menu;
            _heroMenu.Initialize(this, presenter);

            _heroMenu.InstantConceal(true);
        }

        public void ShowMainMenu() => 
            _mainMenu.Reveal(enable: true).Forget();

        public void ShowHeroMenu() => 
            _heroMenu.Reveal(enable: true).Forget();
    }
}