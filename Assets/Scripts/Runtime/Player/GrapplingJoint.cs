using DG.Tweening;
using UnityEngine;

namespace Core.Level
{
    public class GrapplingJoint : MonoBehaviour
    {
        private const float GrappledScaleMultiplier = 0.8f;
        private const float ScaleDuration = 0.4f;

        [SerializeField] private ParticleSystem _particles;
        
        public void OnGrappled()
        {
            if (_particles != null)
                _particles.Stop();

            TweenScale();
        }

        private void TweenScale()
        {
            const int yoyoLoops = 2;

            Vector3 scale = transform.localScale;
            scale *= GrappledScaleMultiplier;
            transform.DOScale(scale, ScaleDuration).SetLoops(yoyoLoops, LoopType.Yoyo);
        }
    }
}