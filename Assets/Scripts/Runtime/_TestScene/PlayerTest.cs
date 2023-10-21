using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private FocusCamera _focusCamera;
    [SerializeField] private MiddleObject _middleObject;
    [SerializeField] private SpringJoint2D _springJoint2D;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameObject _jointObject;
    [SerializeField] private Vector2 _grappleJumpVelocity = new(5f, 8f);

    private Transform _thisTransform;

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

        _middleObject.SetPosition(_thisTransform.position, _jointObject.transform.position);
    }

    private void GrappleJoint()
    {
        _rigidbody2D.velocity = Vector2.right;
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
            _rigidbody2D.AddForce(_grappleJumpVelocity, ForceMode2D.Impulse);

        _lineRenderer.enabled = false;
        _focusCamera.StopFocus();
    }
}
