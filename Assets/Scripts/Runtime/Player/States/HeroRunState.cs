using System;
using System.Threading;
using Core.Common;
using Core.Editor.Debugger;
using Core.Game.Input;
using Core.Level;
using Core.Other;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityTools;

namespace Core.Player
{
    public class HeroRunState : State
    {
        private readonly Hero _hero;
        private readonly InputHandler _inputHandler;
        private readonly CompositeDisposable _disposable = new();
        private readonly CancellationToken _cts;
        private float _nextDashTime = 0;
        private float _acceleration;
        private LookingDirection _direction;
        private OneWayPlatform _currentPlatform;

        private bool IsJumping
        {
            get => _hero.IsJumping.Value;
            set => _hero.IsJumping.Value = value;
        }

        private bool IsDashing
        {
            get => _hero.IsDashing.Value;
            set => _hero.IsDashing.Value = value;
        }

        public HeroRunState(Hero hero, InputHandler inputHandler)
        {
            _hero = hero;
            _inputHandler = inputHandler;
            _cts = _hero.destroyCancellationToken;
        }

        public override void Enter()
        {
            _acceleration = 0f;
            SubscribeInputHandler();
            SubscribeFixedUpdate();
            SubscribePhysicsCallbacks();
        }

        public override void Exit()
        {
            _disposable.Clear();

            _hero.IsRunning.Value = false;
            IsDashing = false;
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
                .Subscribe(_ => ReduceAcceleration(_cts).Forget())
                .AddTo(_disposable);

            _inputHandler.DashCommand
                .Subscribe(_ => Dash(_cts).Forget())
                .AddTo(_disposable);

            _inputHandler.JumpCommand
                .Subscribe(_ => Jump().Forget())
                .AddTo(_disposable);

            _inputHandler.DownCommand
                .Where(_ => _currentPlatform != null)
                .Subscribe(_ => DescendFromPlaftorm(_cts).Forget())
                .AddTo(_disposable);
        }

        private void SubscribeFixedUpdate()
        {
            IObservable<Unit> fixedUpdate = _hero.FixedUpdateAsObservable();
            fixedUpdate
                .Subscribe(_ => Run())
                .AddTo(_disposable);
        }

        private void SubscribePhysicsCallbacks()
        {
            IObservable<Collision2D> onCollisionEnter2D = _hero.OnCollisionEnter2DAsObservable();
            onCollisionEnter2D
                .Subscribe(collision2D =>
                {
                    Tools.InvokeIfNotNull<OneWayPlatform>(
                        collision2D,
                        component => _currentPlatform = component);
                })
                .AddTo(_disposable);
        }

        private void AccelerateRight()
        {
            _direction = LookingDirection.Right;
            RaiseAcceleration(_cts).Forget();
        }

        private void AccelerateLeft()
        {
            _direction = LookingDirection.Left;
            RaiseAcceleration(_cts).Forget();
        }

        private async UniTaskVoid RaiseAcceleration(CancellationToken token)
        {
            try
            {
                float accelerationTime = _hero.Config.AccelerationTime;
                float maximum = (float)_direction;

                _hero.IsRunning.Value = true;

                float elapsedTime = 0;
                while (elapsedTime < accelerationTime)
                {
                    float delta = elapsedTime / accelerationTime;
                    _acceleration = Mathf.Lerp(0f, maximum, delta);
                    elapsedTime += Time.deltaTime;

                    await UniTask.NextFrame(token);
                }

                _acceleration = maximum;
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(HeroRunState)}::{nameof(RaiseAcceleration)}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        private async UniTaskVoid ReduceAcceleration(CancellationToken token)
        {
            try
            {
                float accelerationTime = _hero.Config.AccelerationTime;

                _hero.IsRunning.Value = false;
                float original = _acceleration;

                float elapsedTime = 0;
                while (elapsedTime < accelerationTime)
                {
                    float delta = elapsedTime / accelerationTime;
                    _acceleration = Mathf.Lerp(original, 0f, delta);
                    elapsedTime += Time.deltaTime;

                    await UniTask.NextFrame(token);
                }

                _acceleration = 0f;
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(HeroRunState)}::{nameof(ReduceAcceleration)}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        private void Run()
        {
            if (IsDashing == true || IsJumping == true)
                return;

            HeroConfig config = _hero.Config;
            Vector2 runVelocity = _hero.Rigidbody2D.velocity;
            runVelocity.x = config.RunSpeed * _acceleration;

            _hero.Rigidbody2D.velocity = runVelocity;
        }

        private async UniTaskVoid Dash(CancellationToken token)
        {
            if (Time.time < _nextDashTime || IsDashing == true)
                return;

            _hero.DashedCommand.Execute();
            HeroConfig config = _hero.Config;
            Rigidbody2D rigidbody2D = _hero.Rigidbody2D;

            Vector2 dashVelocity = new(config.DashForce, 0f);
            float initialGravityScale = rigidbody2D.gravityScale;
            
            float directionMultiplier = (float)_direction;
            rigidbody2D.velocity = dashVelocity * directionMultiplier;
            rigidbody2D.gravityScale = 0f;
            IsDashing = true;
            
            await UniTaskUtility.Delay(config.DashDuration, token);

            rigidbody2D.gravityScale = initialGravityScale;
            IsDashing = false;

            _nextDashTime = Time.time + config.DashCooldown;
        }

        private async UniTaskVoid Jump()
        {
            if (IsJumping == true || IsDashing == true)
                return;

            CancellationToken token = _hero.destroyCancellationToken;
            Rigidbody2D rigidbody2D = _hero.Rigidbody2D;
            float duration = _hero.Config.JumpDuration;

            IsJumping = true;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                float progress = _hero.Config.JumpProgression.Evaluate(delta);
                
                Vector2 jumpVector = GetJumpVector();
                Vector2 velocity = GetJumpVelocity(jumpVector, progress);
                rigidbody2D.velocity = velocity;
                
                elapsedTime += Time.deltaTime;
                
                await UniTask.NextFrame(token);
            }

            await UniTask.WaitUntil(
                () => Mathf.Approximately(rigidbody2D.velocity.y, 0f) == true,
                cancellationToken: token);

            IsJumping = false;
        }

        private async UniTaskVoid DescendFromPlaftorm(CancellationToken token)
        {
            Collider2D platformCollider = _currentPlatform.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_hero.Collider2D, platformCollider, true);

            await UniTaskUtility.Delay(_hero.Config.DescendDuration, token);

            Physics2D.IgnoreCollision(_hero.Collider2D, platformCollider, false);
            _currentPlatform = null;
        }

        private Vector2 GetJumpVelocity(Vector2 jumpVector, float progress)
        {
            HeroConfig config = _hero.Config;
            float jumpX = jumpVector.x * config.JumpForce;
            float jumpY = jumpVector.y * config.JumpForce * progress;
            return new(jumpX, jumpY);
        }

        private Vector2 GetJumpVector()
        {
            Vector2 jumpVector = _hero.Config.JumpVector;
            jumpVector.x *= (float)_direction;
            return jumpVector;
        }
    }
}