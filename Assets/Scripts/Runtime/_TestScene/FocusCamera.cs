using System;
using Cinemachine;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera _cinemachine;
    [SerializeField] private MiddleObject _focusObject;
    [SerializeField] private Transform _defaultFollowObject;

    [Header("Configuration")]
    [SerializeField] private Transform _toFocus1;
    [SerializeField] private Transform _toFocus2;
    [SerializeField] private float _fovMargin = 0f;

    private float _aspectRatio;
    public bool _focusing = false;

    #region MonoBehaviour

    private void Awake()
    {
        _aspectRatio = Screen.width / Screen.height;
    }

    private void Update()
    {
        if (_focusing == true)
            FocusBentween(_toFocus1.position, _toFocus2.position);
    }

    #endregion

    public void StartFocus()
    {
        _focusing = true;
        _cinemachine.Follow = _focusObject.transform;
    }

    public void StopFocus()
    {
        _focusing = false;
        _cinemachine.Follow = _defaultFollowObject;
    }

    public void SetFocuses(Transform focus1, Transform focus2)
    {
        if (focus1 == null || focus2 == null)
            throw new ArgumentNullException(
                "Objects to focus cannot be null! 1: {focus1}, 2: {focus2}");

        _toFocus1 = focus1;
        _toFocus2 = focus2;
    }

    private void FocusBentween(Vector2 position1, Vector2 position2)
    {
        _focusObject.SetPosition(position1, position2);

        float fov = CalcualteFovBetween(position1, position2);
        _cinemachine.m_Lens.OrthographicSize = fov;
    }

    private float CalcualteFovBetween(Vector2 position1, Vector2 position2, float margin = 0f)
    {
        Vector3 middlePoint = _focusObject.transform.position;
        float distanceBetweenPlayers = (position2 - position1).magnitude;
        float distanceFromMiddlePoint = (_cinemachine.transform.position - middlePoint).magnitude;

        return (2.0f * Mathf.Rad2Deg * Mathf.Atan(0.5f * distanceBetweenPlayers / (distanceFromMiddlePoint * _aspectRatio))) + margin;
    }
}
