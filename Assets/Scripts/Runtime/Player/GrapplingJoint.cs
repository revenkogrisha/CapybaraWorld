using DG.Tweening;
using UnityEditorInternal;
using UnityEngine;

namespace Core.Level
{
    public class GrapplingJoint : MonoBehaviour
    {
        private const float GrappledScaleMultiplier = 0.8f;
        private const float ScaleDuration = 0.4f;

        [SerializeField] private ParticleSystem _particles;

        private bool _isTweening = false;
        
        public void OnGrappled()
        {
            if (_particles != null)
                _particles.Stop();
            
            if (_isTweening == false)
                TweenScale();
        }

        private void TweenScale()
        {
            const int yoyoLoops = 2;
            _isTweening = true;

            Vector3 scale = transform.localScale;
            scale *= GrappledScaleMultiplier;

            DOTween.Sequence()
                .Append(transform.DOScale(scale, ScaleDuration).SetLoops(yoyoLoops, LoopType.Yoyo))
                .AppendCallback(() => _isTweening = false);
        }
    }
}