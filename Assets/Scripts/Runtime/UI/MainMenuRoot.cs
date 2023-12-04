using Core.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        private MainMenu _mainMenu;
        private UpgradeMenu _upgradeMenu;
        private UIProvider _uiProvider;

        [Inject]
        private void Construct(UIProvider uiProvider) => 
            _uiProvider = uiProvider;

        public void Initialize()
        {
            _mainMenu = _uiProvider.CreateMainMenu(_rectTransform);
            _mainMenu.InitializeRoot(this);
            
            _upgradeMenu = _uiProvider.CreateUpgradeMenu(_rectTransform);
            _upgradeMenu.InitializeRoot(this);

            _upgradeMenu.InstantConceal(true);
        }

        public void ShowMainMenu() => 
            _mainMenu.Reveal(enable: true).Forget();

        public void ShowUpgradeMenu() => 
            _upgradeMenu.Reveal(enable: true).Forget();
    }
}