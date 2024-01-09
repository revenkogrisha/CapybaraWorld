using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "New Skin", menuName = "Presets/Hero Skin")]
    public class SkinPreset : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private SkinName _name;
        [SerializeField] private Sprite _menuItem;
        
        [Header("General Sprites")]
        [SerializeField] private Sprite _head;
        [SerializeField] private Sprite _body;

        [Header("Arm Sprites")]
        [SerializeField] private Sprite _leftArm;
        [SerializeField] private Sprite _rightArm;

        [Header("Leg Sprite")]
        [SerializeField] private Sprite _leg;

        public SkinName Name => _name;
        public Sprite Head => _head;
        public Sprite MenuItem => _menuItem;
        public Sprite Body => _body;
        public Sprite LeftArm => _leftArm;
        public Sprite RightArm => _rightArm;
        public Sprite Leg => _leg;
    }
}