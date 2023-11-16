using System;
using System.Threading;
using Core.Common;
using Core.Game.Input;
using Core.Infrastructure;
using Core.Other;
using Cysharp.Threading.Tasks;
using TMPro;
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
        private CancellationTokenSource _cancellationTokenSource;
        private float _nextDashTime = 0;
        private bool _dashing;
        private bool _jumping;
        private float _acceleration;
        private LookingDirection _direction;

        public HeroRunState(Hero hero, InputHandler inputHandler)
        {
            _hero = hero;
            _inputHandler = inputHandler;
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
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

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
            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;

            _hero.IsRunning.Value = true;

            float maximum = (float)_direction;

            float elapsedTime = 0;
            bool canceled = false;
            while (canceled == false && elapsedTime < accelerationTime)
            {
                float delta = elapsedTime / accelerationTime;
                _acceleration = Mathf.Lerp(0f, maximum, delta);
                elapsedTime += Time.deltaTime;

                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTaskVoid ReduceAcceleration()
        {
            float accelerationTime = _hero.Config.AccelerationTime;
            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;

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
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private void Run()
        {
            if (_dashing == true || _jumping == true)
                return;

            PlayerConfig config = _hero.Config;
            Vector2 runVelocity = _hero.Rigidbody2D.velocity;
            runVelocity.x = config.RunSpeed * _acceleration;

            _hero.Rigidbody2D.velocity = runVelocity;
        }

        private async UniTaskVoid Dash()
        {
            if (Time.time < _nextDashTime || _dashing == true)
                return;

            _hero.DashedCommand.Execute();
            PlayerConfig config = _hero.Config;
            Rigidbody2D rigidbody2D = _hero.Rigidbody2D;

            Vector2 dashVelocity = new(config.DashForce, 0f);
            float initialGravityScale = rigidbody2D.gravityScale;
            
            float directionMultiplier = (float)_direction;
            rigidbody2D.velocity = dashVelocity * directionMultiplier;
            rigidbody2D.gravityScale = 0f;
            _dashing = true;
            
            await MyUniTask
                .Delay(config.DashDuration, _hero.destroyCancellationToken)
                .SuppressCancellationThrow();

            rigidbody2D.gravityScale = initialGravityScale;
            _dashing = false;

            _nextDashTime = Time.time + config.DashCooldown;
        }

        private async UniTaskVoid Jump()
        {
            if (_jumping == true || _dashing == true)
                return;

            CancellationToken token = _hero.destroyCancellationToken;
            PlayerConfig config = _hero.Config;
            float duration = config.JumpDuration;
            _jumping = true;

            float elapsedTime = 0f;
            bool canceled = false;
            while (canceled == false && elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                float progress = config.JumpProgression.Evaluate(delta);
                
                Vector2 velocity = config.JumpVector * config.JumpForce * progress;
                _hero.Rigidbody2D.velocity = velocity;
                elapsedTime += Time.deltaTime;
                
                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }

            _jumping = false;
        }
    }
    // TODO: fixed update, destroy token, player config
}