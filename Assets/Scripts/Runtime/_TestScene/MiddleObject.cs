using UnityEngine;

public class MiddleObject : MonoBehaviour
{
    private Transform _thisTransform;

    private void Awake()
    {
        _thisTransform = transform;
    }

    public void SetPosition(Vector2 position1, Vector2 position2)
    {
        Vector2 middlePoint = position1 + 0.5f * (position2 - position1);
        _thisTransform.position = middlePoint;
    }
}