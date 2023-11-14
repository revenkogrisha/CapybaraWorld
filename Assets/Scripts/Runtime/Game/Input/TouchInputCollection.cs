using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Game.Input
{
    [CreateAssetMenu(fileName = "Touch Input Collection", menuName = "Collections/Touch Input")]
    public class TouchInputCollection : ScriptableObject
    {
        [SerializeField] private InputActionReference _holdAction;
        [SerializeField] private InputActionReference _positionAction;
        [SerializeField] private InputActionReference _dashAction;

        public InputActionReference HoldAction => _holdAction;
        public InputActionReference PositionAction => _positionAction;
        public InputActionReference DashAction => _dashAction;
    }
}
