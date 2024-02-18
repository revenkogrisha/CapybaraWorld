using Core.Audio;
using Core.Common;
using Core.Other;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Level
{
    public class Enemy : Entity
	{
        [SerializeField] private bool _isImmortal = false;

        [Header("Death Tweening")]
        [SerializeField] private float _deathTweenDuration = 0.3f;
        [SerializeField] private Vector2 _scaleVectorOne = new(1f, 0.7f);
        [SerializeField] private Vector2 _scaleVectorTwo = new(0.7f, 1f);
        
        private ParticlesHelper _particlesHelper;
        private IAudioHandler _audioHandler;

        public LookingDirection Direction { get; set; }
        [HideInInspector] public ReactiveProperty<bool> IsDead = new();

        [Inject]
        private void BaseConstruct(ParticlesHelper particlesHelper, IAudioHandler audioHandler)
        {
            _particlesHelper = particlesHelper;
            _audioHandler = audioHandler;
        }

        public bool TryPerformDeath()
        {
            if (_isImmortal == true)
                return false;

            if (IsDead.Value == true)
                return IsDead.Value;

            IsDead.Value = true;

            float direction = (float)Direction;

            _scaleVectorOne.x *= direction;
            _scaleVectorTwo.x *= direction;
            
            transform.localScale = new Vector2(direction, 1f);

            DOTween.Sequence()
                .Append(transform.DOScale(_scaleVectorOne, _deathTweenDuration))
                .Append(transform.DOScale(_scaleVectorTwo, _deathTweenDuration))
                .Append(transform.DOScale(Vector2.zero, _deathTweenDuration))
                .AppendCallback(() => 
                {
                    _particlesHelper
                        .Spawn(ParticlesName.EnemyDeath, transform.position)
                        .Forget();
                        
                    _audioHandler.PlaySound(AudioName.EnemyDeath);

                    gameObject.SelfDestroy();
                });
            
                return true;
        }
    }
}