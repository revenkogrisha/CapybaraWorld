using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "UI Collection", menuName = "Collections/UI Collection")]
    public class UICollection : ScriptableObject
    {
        [field: SerializeField] public GameObject MainMenuPrefab { get; private set; }
    }
}
