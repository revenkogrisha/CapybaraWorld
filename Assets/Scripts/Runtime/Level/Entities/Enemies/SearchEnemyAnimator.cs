using UniRx;
using UnityEngine;

namespace Core.Level
{
    public class SearchEnemyAnimator : MonoBehaviour
    {
        [SerializeField] private SearchEnemyBase _enemy;
        [SerializeField] private Animator _animator;
        
        private readonly CompositeDisposable _disposable = new();
        private readonly int _triggeredHash = Animator.StringToHash("Triggered");

        private void OnEnable()
        {
            _enemy.IsTriggered
                .Subscribe(value => _animator.SetBool(_triggeredHash, value))
                .AddTo(_disposable);
        }

        private void OnDisable() => 
            _disposable.Clear();
    }
}
