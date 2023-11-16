using System;
using System.Threading;
using Core.Common;
using Core.Game.Input;
using Core.Infrastructure;
using Core.Other;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Player
{
    public class HeroRunState : State
    {
        private readonly Hero _hero;
        private readonly InputHandler _inputHandler;
        private readonly CompositeDisposable _disposable = new();
        private CancellationToken _cancellationToken;
        private float _nextDashTime = 0;
        private bool _dashing;
        private float _acceleration;
        private LookingDirection _direction;

        private bool IsJumping
        {
            get => _hero.IsJumping.Value;
            set => _hero.IsJumping.Value = value;
        }

        public HeroRunState(Hero hero, InputHandler inputHandler)
        {
            _hero = hero;
            _inputHandler = inputHandler;
            _cancellationToken = _hero.destroyCancellationToken;
        }

        public override void Enter()
        {
            _acceleration = 0f;
            SubscribeInputHandler();
            SubscribeFixedUpdate();
        }

        public override void Exit()
        {
            _disposable.Clear();

            _hero.IsRunning.Value = false;
            _dashing = false;
        }

        private void SubscribeInputHandler()
        {
            _inputHandler.MoveRightCommand
                .Subscribe(_ => AccelerateRight())
                .AddTo(_disposable);

            _inputHandler.MoveLeftCommand
                .Subscribe(_ => AccelerateLeft())
                .AddTo(_disposable);

            _inputHandler.StopCommand
                .Subscribe(_ => ReduceAcceleration().Forget())
                .AddTo(_disposable);

            _inputHandler.DashCommand
                .Subscribe(_ => Dash().Forget())
                .AddTo(_disposable);

            _inputHandler.JumpCommand
                .Subscribe(_ => Jump().Forget())
                .AddTo(_disposable);
        }

        private void SubscribeFixedUpdate()
        {
            IObservable<Unit> fixedUpdate = _hero.FixedUpdateAsObservable();
            fixedUpdate
                .Subscribe(_ => Run())
                .AddTo(_disposable);
        }

        private void AccelerateRight()
        {
            _direction = LookingDirection.Right;
            RaiseAcceleration().Forget();
        }

        private void AccelerateLeft()
        {
            _direction = LookingDirection.Left;
            RaiseAcceleration().Forget();
        }

        private async UniTaskVoid RaiseAcceleration()
        {
            float accelerationTime = _hero.Config.AccelerationTime;
            float maximum = (float)_direction;

            _hero.IsRunning.Value = true;

            float elapsedTime = 0;
            bool canceled = false;
            while (canceled == false && elapsedTime < accelerationTime)
            {
                float delta = elapsedTime / accelerationTime;
                _acceleration = Mathf.Lerp(0f, maximum, delta);
                elapsedTime += Time.deltaTime;

                canceled = await UniTask
                    .NextFrame(_cancellationToken)
                    .SuppressCancellationThrow();
            }
        }

        private async UniTaskVoid ReduceAcceleration()
        {
            float accelerationTime = _hero.Config.AccelerationTime;

            _hero.IsRunning.Value = false;
            float original = _acceleration;

            float elapsedTime = 0;
            bool canceled = false;
            while (canceled == false && elapsedTime < accelerationTime)
            {
                float delta = elapsedTime / accelerationTime;
                _acceleration = Mathf.Lerp(original, 0f, delta);
                elapsedTime += Time.deltaTime;

                canceled = await UniTask
                    .NextFrame(_cancellationToken)
                    .SuppressCancellationThrow();
            }
        }

        private void Run()
        {
            if (_dashing == true || IsJumping == true)
                return;

            HeroConfig config = _hero.Config;
            Vector2 runVelocity = _hero.Rigidbody2D.velocity;
            runVelocity.x = config.RunSpeed * _acceleration;

            _hero.Rigidbody2D.velocity = runVelocity;
        }

        private async UniTaskVoid Dash()
        {
            if (Time.time < _nextDashTime || _dashing == true)
                return;

            _hero.DashedCommand.Execute();
            HeroConfig config = _hero.Config;
            Rigidbody2D rigidbody2D = _hero.Rigidbody2D;

            Vector2 dashVelocity = new(config.DashForce, 0f);
            float initialGravityScale = rigidbody2D.gravityScale;
            
            float directionMultiplier = (float)_direction;
            rigidbody2D.velocity = dashVelocity * directionMultiplier;
            rigidbody2D.gravityScale = 0f;
            _dashing = true;
            
            await MyUniTask
                .Delay(config.DashDuration, _cancellationToken)
                .SuppressCancellationThrow();

            rigidbody2D.gravityScale = initialGravityScale;
            _dashing = false;

            _nextDashTime = Time.time + config.DashCooldown;
        }

        private async UniTaskVoid Jump()
        {
            if (IsJumping == true || _dashing == true)
                return;

            CancellationToken token = _hero.destroyCancellationToken;
            HeroConfig config = _hero.Config;
            Rigidbody2D rigidbody2D = _hero.Rigidbody2D;
            float duration = config.JumpDuration;

            IsJumping = true;

            float elapsedTime = 0f;
            bool canceled = false;
            while (canceled == false && elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                float progress = config.JumpProgression.Evaluate(delta);
                
                Vector2 velocity = config.JumpVector * config.JumpForce * progress;
                rigidbody2D.velocity = velocity;
                elapsedTime += Time.deltaTime;
                
                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }

            await UniTask
                .WaitUntil(() => Mathf.Approximately(rigidbody2D.velocity.y, 0f))
                .SuppressCancellationThrow();

            IsJumping = false;
        }
    }
}