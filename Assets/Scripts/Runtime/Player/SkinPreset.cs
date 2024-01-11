using TriInspector;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "New Skin", menuName = "Presets/Hero Skin")]
    public class SkinPreset : ScriptableObject
    {
        [Title("General")]
        [InfoBox("Supposed to be single value and not 'Nothing'!")]
        [SerializeField] private SkinName _name;
        [SerializeField] private Sprite _menuItem;

        [Space]
        [SerializeField, Min(0)] private int _foodCost = 15;
        
        [Title("General Sprites")]
        [SerializeField] private Sprite _head;
        [SerializeField] private Sprite _body;

        [Title("Arm Sprites")]
        [SerializeField] private Sprite _leftArm;
        [SerializeField] private Sprite _rightArm;

        [Title("Leg Sprite")]
        [SerializeField] private Sprite _leg;


        public SkinName Name => _name;
        public Sprite Head => _head;
        public Sprite MenuItem => _menuItem;
        public int FoodCost => _foodCost;

        public Sprite Body => _body;
        public Sprite LeftArm => _leftArm;
        public Sprite RightArm => _rightArm;

        public Sprite Leg => _leg;
    }
}