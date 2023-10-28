using Core.UI;
using UnityEngine;

namespace Core.Factories
{
    public class UIProvider
    {
        private readonly UICollection _collection;
        private readonly Transform _root;

        public UIProvider(UICollection collection, Transform root)
        {
            _collection = collection;
            _root = root;
        }

        public GameObject CreateMainMenu()
        {
            GameObject mainMenu = Object.Instantiate(_collection.MainMenuPrefab);
            mainMenu.transform.SetParent(_root);
            
            return mainMenu;
        }
    }
}
