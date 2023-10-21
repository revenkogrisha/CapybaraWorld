using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera _cinemachine;
    [SerializeField] private Transform _followObject;

    [Header("Configuration")]
    [SerializeField, Min(0f)] private float _regularFov = 10f;
    [SerializeField, Min(0f)] private float _focusFov = 5f;
    [SerializeField, Range(0f, 1f)] private float _fovChangeDuration = 0.3f;

    private bool _focusing = false;

    private float Fov
    {
        get => _cinemachine.m_Lens.OrthographicSize;
        set => _cinemachine.m_Lens.OrthographicSize = value;
    }

    private void Awake()
    {
        _cinemachine.Follow = _followObject;
    }

    public void StartFocus(Transform toFocus)
    {
        if (_focusing == true)
            return;

        _focusing = true;
        _cinemachine.Follow = toFocus;
        StartCoroutine(ChangeFov(_focusFov, _fovChangeDuration));
    }

    public void StopFocus()
    {
        if (_focusing == false)
            return;

        _focusing = false;
        _cinemachine.Follow = _followObject;
        StartCoroutine(ChangeFov(_regularFov, _fovChangeDuration));
    }

    private IEnumerator ChangeFov(float targetFov, float duration)
    {
        float elapsedTime = 0f;
        float currentFov = Fov;
        while (elapsedTime < duration)
        {
            float delta = elapsedTime / duration;
            Fov = Mathf.Lerp(currentFov, targetFov, delta);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
