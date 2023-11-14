using System;
using System.Threading;
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
        private const float AccelerationMaximum = 1f;

        private readonly Hero _hero;
        private readonly CompositeDisposable _disposable = new();
        private CancellationTokenSource _cancellationTokenSource;
        private float _nextDashTime = 0;
        private bool _dashing;
        private float _acceleration;

        private bool IsStateActive => FiniteStateMachine.CompareState<HeroRunState>();

        public HeroRunState(Hero hero) =>
            _hero = hero;

        public override void Enter()
        {
            _acceleration = 0f;
            SubscribeUpdates();
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

        private void SubscribeUpdates()
        {
            const float doubleClickInterval = 0.4f;

            IObservable<Unit> update = _hero.UpdateAsObservable();
            update
                .Where(_ => IsStateActive == true)
                .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
                .Subscribe(_ => RaiseAcceleration().Forget())
                .AddTo(_disposable);

            update
                .Where(_ => IsStateActive == true)
                .Where(_ => Input.GetKeyUp(KeyCode.Mouse0))
                .Subscribe(_ => ReduceAcceleration().Forget())
                .AddTo(_disposable);

            update
                .Where(_ => IsStateActive == true)
                .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
                .Buffer(TimeSpan.FromSeconds(doubleClickInterval))
                .Where(x => x.Count > 1)
                .Subscribe(_ => Dash().Forget())
                .AddTo(_disposable);
            
            IObservable<Unit> fixedUpdate = _hero.FixedUpdateAsObservable();
            fixedUpdate
                .Where(_ => IsStateActive == true)
                .Subscribe(_ => Run())
                .AddTo(_disposable);
        }

        private async UniTaskVoid RaiseAcceleration()
        {
            float accelerationTime = _hero.Config.AccelerationTime;
            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;

            _hero.IsRunning.Value = true;

            float elapsedTime = 0;
            bool canceled = false;
            while (canceled == false && elapsedTime < accelerationTime)
            {
                float delta = elapsedTime / accelerationTime;
                _acceleration = Mathf.Lerp(0f, AccelerationMaximum, delta);
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

            float elapsedTime = 0;
            bool canceled = false;
            while (canceled == false && elapsedTime < accelerationTime)
            {
                float delta = elapsedTime / accelerationTime;
                _acceleration = Mathf.Lerp(AccelerationMaximum, 0f, delta);
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
            if (_dashing == true)
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

            rigidbody2D.velocity = dashVelocity;
            rigidbody2D.gravityScale = 0f;
            _dashing = true;
            
            await MyUniTask
                .Delay(config.DashDuration, _hero.destroyCancellationToken)
                .SuppressCancellationThrow();

            rigidbody2D.gravityScale = initialGravityScale;
            _dashing = false;

            _nextDashTime = Time.time + config.DashCooldown;
        }
    }
}