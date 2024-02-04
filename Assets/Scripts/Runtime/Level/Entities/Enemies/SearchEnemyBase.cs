using System;
using Core.Common;
using UniRx;
using UnityEngine;

namespace Core.Level
{
    public abstract class SearchEnemyBase : Enemy
    {
        [Header("Search Base Components")]
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SearchEnemyPreset _searchPreset;

        [HideInInspector] public readonly ReactiveProperty<bool> IsTriggered = new();

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

        public void OnEnable()
        {
            StateMachine.ChangeState<EnemySearchingState>();
            transform.localScale = Vector2.one;
            Direction = LookingDirection.Left;
        }

        private void OnDisable()
        {
            StateMachine.ChangeState<InactiveState>();
            _disposable.Clear();
        }

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
