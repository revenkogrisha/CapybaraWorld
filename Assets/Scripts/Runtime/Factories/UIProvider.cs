using Core.UI;
using Zenject;

namespace Core.Factories
{
    public class UIProvider
    {
        private readonly UICollection _collection;
        private readonly IUIRoot _root;
        private readonly DiContainer _diContainer;

        [Inject]
        public UIProvider(UICollection collection, IUIRoot uiRoot, DiContainer diContainer)
        {
            _collection = collection;
            _root = uiRoot;
            _diContainer = diContainer;
        }

        public MainMenu CreateMainMenu()
        {
            MainMenu mainMenu = _diContainer
                .InstantiatePrefabForComponent<MainMenu>(
                    _collection.MainMenuPrefab,
                     _root.RectTransform);
            
            return mainMenu;
        }
    }
}