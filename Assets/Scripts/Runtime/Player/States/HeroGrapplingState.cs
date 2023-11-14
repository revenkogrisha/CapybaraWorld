using System;
using System.Linq;
using Core.Game.Input;
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
        private readonly InputHandler _inputHandler;
        private readonly CompositeDisposable _disposable = new();
        private GrapplingJoint _jointObject;
        private bool _isGrappling;

        private bool IsStateActive => FiniteStateMachine.CompareState<HeroGrapplingState>();

        public HeroGrapplingState(Hero hero, InputHandler inputHandler)
        {
            _hero = hero;
            _heroTransform = hero.transform;
            _inputHandler = inputHandler;
        }

        public override void Enter()
        {
            SubscribeInputHandler();
            SubscribeUpdate();
        }

        public override void Exit() =>
            _disposable.Clear();

        private void SubscribeInputHandler()
        {
            _inputHandler.HoldStartCommand
                .Subscribe(_ => GrappleJoint())
                .AddTo(_disposable);

            _inputHandler.HoldEndCommand
                .Subscribe(_ => ReleaseJoint())
                .AddTo(_disposable);
        }

        private void SubscribeUpdate()
        {
            IObservable<Unit> update = _hero.UpdateAsObservable();
            update
                .Where(_ => _isGrappling == true)
                .Subscribe(_ => _hero.Rope.Draw(_jointObject.transform.position))
                .AddTo(_disposable);
        }

        private void GrappleJoint()
        {
            if (_isGrappling == true || IsStateActive == false)
                return;

            bool canGrapple = TryFindNearestJoint();
            if (canGrapple == false)
                return;

            EnableGrappling(true);
            _hero.Rigidbody2D.velocity *= Vector2.right * 0.8f;
            Vector2 jointPosition = _jointObject.transform.position;
            _hero.SpringJoint2D.connectedAnchor = jointPosition;

            _hero.GrappledJoint.Value = _jointObject.transform;
        }

        private void ReleaseJoint()
        {
            if (_isGrappling == false)
                return;

            EnableGrappling(false);
            Vector2 velocity = _hero.Rigidbody2D.velocity;
            if (velocity.x > 0 && velocity.y >= 0)
                _hero.Rigidbody2D.velocity *= _hero.Config.GrappleJumpVelocityMultiplier;

            _hero.GrappledJoint.Value = null;
        }

        private bool TryFindNearestJoint()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                _heroTransform.position,
                _hero.Config.GrappleRadius,
                _hero.Config.JointLayer);

            if (colliders.IsNullOrEmpty() == true)
                return false;

            Func<Collider2D, float> orderFunc = collider => 
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
            
            _hero.Rope.enabled = value;
        }
    }

}
