using TriInspector;
using UnityEngine;

namespace Core.Player
{
    public class HeroSkinSetter : MonoBehaviour
    {
        [Header("General Sprite Renderers")]
        [SerializeField, Required] private SpriteRenderer _head;
        [SerializeField, Required] private SpriteRenderer _body;

        [Header("Arms Sprite Renderers")]
        [SerializeField, Required] private SpriteRenderer _leftArm;
        [SerializeField, Required] private SpriteRenderer _rightArm;

        [Header("Legs Sprite Renderers")]
        [SerializeField, Required] private SpriteRenderer _leftLeg;
        [SerializeField, Required] private SpriteRenderer _rightLeg;

        [DisableInEditMode]
        [Button(buttonSize: ButtonSizes.Medium)]
        public void Set(SkinPreset preset)
        {
            _head.sprite = preset.Head;
            _body.sprite = preset.Body;

            _leftArm.sprite = preset.LeftArm;
            _rightArm.sprite = preset.RightArm;

            _leftLeg.sprite = preset.Leg;
            _rightLeg.sprite = preset.Leg;
        }
    }
}
