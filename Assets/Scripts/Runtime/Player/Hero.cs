using System;
using Core.Infrastructure;
using Core.Level;
using UnityEngine;
using UnityTools;

namespace Core.Player
{
    public class Hero : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SpringJoint2D _springJoint2D;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private PlayerConfig _config;
        private IFiniteStateMachine _stateMachine;
        private MiddleObject _middleObject;

        public SpringJoint2D SpringJoint2D => _springJoint2D;
        public LineRenderer LineRenderer => _lineRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public MiddleObject MiddleObject => _middleObject;
        public PlayerConfig Config => _config;

        public event Action<Transform> JointGrappled;
        public event Action JointReleased;
        public event Action Died;

        #region MonoBehaviour

        private void Awake()
        {
            _springJoint2D.enabled = false;
            _lineRenderer.enabled = false;
            InitialzeStateMachine();
        }

        private void OnDisable()
        {
            if (_middleObject != null)
            {
                _middleObject = null;
                Destroy(_middleObject);
            }
        }

        private void Update() =>
            _stateMachine.DoStateUpdate();

        private void OnTriggerEnter2D(Collider2D other) => 
            Tools.InvokeIfNotNull<DeadlyForPlayerObject>(other, NotifyOnDeath);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _config.GrappleRadius);
        }

        #endregion

        public void Initialize(PlayerConfig config, MiddleObject middleObject)
        {
            _config = config;
            _middleObject = middleObject;
            _stateMachine.ChangeState<HeroGrapplingState>();
        }

        public void NotifyOnJointGrappled() =>
            JointGrappled?.Invoke(_middleObject.transform);

        public void NotifyOnJointReleased() =>
            JointReleased?.Invoke();

        private void InitialzeStateMachine()
        {
            _stateMachine = new FiniteStateMachine();

            HeroGrapplingState grapplingState = new(this);

            _stateMachine.AddState<HeroGrapplingState>(grapplingState);
        }

        private void NotifyOnDeath() => 
            Died?.Invoke();
    }
}
