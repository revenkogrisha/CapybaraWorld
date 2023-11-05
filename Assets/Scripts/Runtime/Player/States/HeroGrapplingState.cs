using System.Linq;
using Core.Infrastructure;
using Core.Level;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityTools;

namespace Core.Player
{
    public class HeroGrapplingState : State
    {
        private readonly Hero _hero;
        private readonly Transform _heroTransform;

        private GrapplingJoint _jointObject;
        private bool _isGrappling;

        public HeroGrapplingState(Hero hero)
        {
            _hero = hero;
            _heroTransform = hero.transform;

            SubscribeUpdate();
        }

        private void SubscribeUpdate()
        {
            System.IObservable<Unit> update = _hero.UpdateAsObservable();
            update
                .Where(_ => Input.GetKeyDown(KeyCode.Mouse0) == true)
                .Subscribe(_ => GrappleJoint());

            update
                .Where(_ => Input.GetKeyUp(KeyCode.Mouse0) == true)
                .Subscribe(_ => ReleaseJoint());

            update
                .Where(_ => _hero.SpringJoint2D.enabled == true)
                .Subscribe(_ => 
                    _hero.LineRenderer.SetPosition(1, _heroTransform.position));

            update
                .Where(_ => _jointObject != null)
                .Subscribe(_ =>
                    _hero.MiddleObject
                        .SetPosition(_heroTransform.position, _jointObject.transform.position));
        }

        private void GrappleJoint()
        {
            if (_isGrappling == true)
                return;

            bool canGrapple = TryFindNearestJoint();
            if (canGrapple == false)
                return;

            EnableGrappling(true);
            _hero.Rigidbody2D.velocity *= Vector2.right * 0.8f;
            Vector2 jointPosition = _jointObject.transform.position;
            _hero.SpringJoint2D.connectedAnchor = jointPosition;

            _hero.LineRenderer.SetPosition(0, jointPosition);

            _hero.NotifyOnJointGrappled();
        }

        private void ReleaseJoint()
        {
            if (_isGrappling == false)
                return;

            EnableGrappling(false);
            Vector2 velocity = _hero.Rigidbody2D.velocity;
            if (velocity.x > 0 && velocity.y >= 0)
                _hero.Rigidbody2D.velocity *= _hero.Config.GrappleJumpVelocityMultiplier;

            _hero.NotifyOnJointReleased();
        }

        private bool TryFindNearestJoint()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                _heroTransform.position,
                _hero.Config.GrappleRadius,
                _hero.Config.JointLayer);

            if (colliders.IsNullOrEmpty() == true)
                return false;

            System.Func<Collider2D, float> orderFunc = collider => 
                (_heroTransform.position - collider.transform.position).sqrMagnitude;

            Collider2D nearest = colliders
                .OrderBy(orderFunc)
                .First();

            _jointObject = nearest.GetComponent<GrapplingJoint>();
            return _jointObject != null;
        }

        private void EnableGrappling(bool value)
        {
            _hero.SpringJoint2D.enabled = value;
            _hero.LineRenderer.enabled = value;
            _isGrappling = value;
        }
    }

}
