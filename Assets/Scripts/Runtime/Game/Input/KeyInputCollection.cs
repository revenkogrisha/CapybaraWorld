using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Game.Input
{
    [CreateAssetMenu(fileName = "Key Input Collection", menuName = "Collections/Key Input")]
    public class KeyInputCollection : ScriptableObject
    {
        [SerializeField] private InputActionReference _movementAction;
        [SerializeField] private InputActionReference _descendAction;
        [SerializeField] private InputActionReference _dashAction;
        [SerializeField] private InputActionReference _jumpAction;

        public InputActionReference MovementAction => _movementAction;
        public InputActionReference DescendAction => _descendAction;
        public InputActionReference DashAction => _dashAction;
        public InputActionReference JumpAction => _jumpAction;
    }
}
