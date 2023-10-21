using System.Linq;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private FocusCamera _focusCamera;
    [SerializeField] private MiddleObject _middleObject;
    [SerializeField] private SpringJoint2D _springJoint2D;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Grapple Settings")]
    [SerializeField, Range(0f, 100f)] private float _grappleRadius = 10f;
    [SerializeField] private float _grappleJumpVelocityMultiplier = 1.5f;
    [SerializeField] private LayerMask _jointLayer;

    private Transform _thisTransform;
    private GrapplingJoint _jointObject;

    private void Awake()
    {
        _springJoint2D.enabled = false;
        _lineRenderer.enabled = false;
        _thisTransform = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            GrappleJoint();
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            ReleaseJoint();

        if (_springJoint2D.enabled == true)
            _lineRenderer.SetPosition(1, transform.position);

        if (_jointObject != null)
            _middleObject.SetPosition(_thisTransform.position, _jointObject.transform.position);
    }

    private void GrappleJoint()
    {
        bool canGrapple = FindNearestJoint();
        if (canGrapple == false)
            return;

        _rigidbody2D.velocity *= Vector2.right * 0.8f;
        _springJoint2D.enabled = true;
        Vector2 jointPosition = _jointObject.transform.position;
        _springJoint2D.connectedAnchor = jointPosition;

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, jointPosition);
        _focusCamera.StartFocus(_middleObject.transform);
    }

    private void ReleaseJoint()
    {
        _springJoint2D.enabled = false;
        if (_rigidbody2D.velocity.x > 0)
            _rigidbody2D.velocity *= _grappleJumpVelocityMultiplier;

        _lineRenderer.enabled = false;
        _focusCamera.StopFocus();
    }

    private bool FindNearestJoint()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_thisTransform.position, _grappleRadius, _jointLayer);

        if (colliders == null || colliders.Length == 0)
            return false;

        Collider2D nearest = colliders
            .OrderBy(
                collider => (_thisTransform.position - collider.transform.position).sqrMagnitude)
            .First();

        _jointObject = nearest.GetComponent<GrapplingJoint>();
        return _jointObject != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _grappleRadius);
    }
}
