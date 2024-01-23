using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly PlayerUpgrade _upgrade;
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

        public HeroRunState(Hero hero, InputHandler inputHandler, PlayerUpgrade upgrade)
        {
            _hero = hero;
            _inputHandler = inputHandler;
            _upgrade = upgrade;
            
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

                // Acceleration is in range 0f - 1f or -1f; This is to auto determine direction
                float target = (float)_direction;

                _hero.IsRunning.Value = true;

                float elapsedTime = 0;
                while (elapsedTime < accelerationTime)
                {
                    float delta = elapsedTime / accelerationTime;
                    _acceleration = Mathf.Lerp(0f, target, delta);
                    elapsedTime += Time.deltaTime;

                    await UniTask.NextFrame(token);
                }

                _acceleration = target;
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
                float target = 0f;

                float elapsedTime = 0;
                while (elapsedTime < accelerationTime)
                {
                    float delta = elapsedTime / accelerationTime;
                    _acceleration = Mathf.Lerp(original, target, delta);
                    elapsedTime += Time.deltaTime;

                    await UniTask.NextFrame(token);
                }

                _acceleration = target;
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
            try
            {
                if (Time.time < _nextDashTime || IsDashing == true)
                    return;

                _hero.DashedCommand.Execute(_hero.Config.DashCooldown
                    / _upgrade.DashCooldownBonus.Multiplier);

                StartDash();

                await UniTaskUtility.Delay(_hero.Config.DashDuration, token);

                EndDash();

                _nextDashTime = Time.time + _hero.Config.DashCooldown / _upgrade.DashCooldownBonus.Multiplier;
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(HeroRunState)}::{nameof(Dash)}: {ex.Message} \n{ex.StackTrace}");
            }
            finally
            {
                if (IsDashing == true)
                    EndDash();
            }
        }

        private async UniTaskVoid Jump()
        {
            try
            {
                if (IsJumping == true || IsDashing == true)
                    return;

                CancellationToken token = _hero.destroyCancellationToken;

                _hero.PlayParticles();

                await StartJump(token);
                await WaitUntilLanded(token);

                IsJumping = false;
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(HeroRunState)}::{nameof(Jump)}: {ex.Message} \n{ex.StackTrace}");
            }
            finally
            {
                IsJumping = false;
            }
        }

        private async UniTaskVoid DescendFromPlaftorm(CancellationToken token)
        {
            try
            {
                Collider2D platformCollider = _currentPlatform.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(_hero.Collider2D, platformCollider, true);

                await UniTaskUtility.Delay(_hero.Config.DescendDuration, token);

                Physics2D.IgnoreCollision(_hero.Collider2D, platformCollider, false);
                _currentPlatform = null;
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(HeroRunState)}::{nameof(DescendFromPlaftorm)}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        private void StartDash()
        {
            float directionMultiplier = (float)_direction;
            Vector2 dashVelocity = new(_hero.Config.DashForce, 0f);

            _hero.Rigidbody2D.velocity = dashVelocity
                * directionMultiplier
                * _upgrade.DashSpeedBonus.Multiplier;

            _hero.Rigidbody2D.gravityScale = _hero.Config.DashGravityScale;
            IsDashing = true;
        }

        private void EndDash()
        {
            _hero.Rigidbody2D.gravityScale = _hero.Config.DefaultGravityScale;
            IsDashing = false;
        }

        private async UniTask StartJump(CancellationToken token)
        {
            IsJumping = true;

            float elapsedTime = 0f;
            float duration = _hero.Config.JumpDuration;
            while (elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                float progress = _hero.Config.JumpProgression.Evaluate(delta);

                Vector2 jumpVector = GetJumpVector();
                Vector2 velocity = GetJumpVelocity(jumpVector, progress);
                _hero.Rigidbody2D.velocity = velocity;

                elapsedTime += Time.deltaTime;

                await UniTask.NextFrame(token);
            }
        }

        private async Task WaitUntilLanded(CancellationToken token)
        {
            await UniTask.WaitUntil(
                () => Mathf.Approximately(_hero.Rigidbody2D.velocity.y, 0f) == true,
                cancellationToken: token);
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