using System;
using Core.Common;
using NTC.Pool;
using UniRx;
using UnityEngine;

namespace Core.Level
{
    public abstract class SearchEnemyBase : Enemy, ISpawnable, IDespawnable
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Space]
        [SerializeField] private SearchEnemyPreset _searchPreset;

        [HideInInspector] public readonly ReactiveProperty<bool> Triggered = new();

        protected State<Vector2> StateOnTrigger;
		protected IFiniteStateMachine StateMachine;

		private readonly CompositeDisposable _disposable = new();

        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public SearchEnemyPreset SearchPreset => _searchPreset;

		#region MonoBehaviour

		private void Awake()
		{
			InitializeStateMachine();   
            InitializeStates();
		}

        public void OnSpawn() => 
            StateMachine.ChangeState<EnemySearchingState>();

        public void OnDespawn() => 
            StateMachine.ChangeState<InactiveState>();

        private void OnDisable() => 
			_disposable.Clear();

        #endregion

        private void InitializeStateMachine() => 
            StateMachine = new FiniteStateMachine();

        protected virtual void InitializeStates()
        {
            if (StateOnTrigger == null)
                throw new ArgumentNullException("_stateOnTrigger wasn't initialized!");

            InactiveState inactiveState = new();
			EnemySearchingState searchingState = new(this, StateOnTrigger);

			StateMachine.AddState<InactiveState>(inactiveState);
			StateMachine.AddState<EnemySearchingState>(searchingState);
        }
    }
}
