using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityTools.Buttons;

namespace Core.UI
{
    public class UpgradeMenu : AnimatedUI
    {
        [Space]
        [SerializeField] private UIButton _backButton;
        
        private MainMenuRoot _root;

        public void InitializeRoot(MainMenuRoot root) =>
            _root = root;

        private void OnEnable() => 
            _backButton.OnClicked += ToMainMenu;

        private void OnDisable() => 
            _backButton.OnClicked -= ToMainMenu;

        private void ToMainMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowMainMenu();
        }
    }
}