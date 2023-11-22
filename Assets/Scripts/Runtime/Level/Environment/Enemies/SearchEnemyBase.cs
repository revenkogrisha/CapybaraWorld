using System;
using Core.Infrastructure;
using UniRx;
using UnityEngine;

namespace Core.Level
{
    public abstract class SearchEnemyBase : Enemy
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Space]
        [SerializeField] private SearchEnemyPreset _searchPreset;

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

			StateMachine.ChangeState<EnemySearchingState>();
		}

		private void OnDisable() => 
			_disposable.Clear();

        #endregion

        private void InitializeStateMachine() => 
            StateMachine = new FiniteStateMachine();

        protected virtual void InitializeStates()
        {
            if (StateOnTrigger == null)
                throw new ArgumentNullException("_stateOnTrigger wasn't initialized!");

			EnemySearchingState searchingState = new(this, StateOnTrigger);
			StateMachine.AddState<EnemySearchingState>(searchingState);
        }
    }
}
