using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "UI Collection", menuName = "Collections/UI Collection")]
    public class UICollection : ScriptableObject
    {
        [field: SerializeField] public MainMenu MainMenuPrefab { get; private set; }
        [field: SerializeField] public GameOverMenu GameOverMenuPrefab { get; private set; }
    }
}
