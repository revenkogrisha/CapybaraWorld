using UnityEngine;

namespace Core.Player
{
    public class GrapplingRope : MonoBehaviour
//                                                             Refactor this (Mb without monobeh)
    {
        [Header("Components")]
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("General")]
        [SerializeField] private int _percision = 40;
        [SerializeField, Range(0f, 20f)] private float _straightenLineSpeed = 5;

        [Header("Animation Settings")]
        [SerializeField] private AnimationCurve _ropeAnimationCurve;
        [SerializeField, Range(0.01f, 4f)] private float _startWaveSize = 2;

        [Header("Progression Settings")]
        [SerializeField] private AnimationCurve _ropeProgressionCurve;
        [SerializeField, Range(1f, 50f)] private float _ropeProgressionSpeed = 5;
        [SerializeField] private float _strightDuration = 0.5f;

        private float _waveSize = 0;
        private float _moveTime = 0;
        private bool _strightLine = true;
        private Vector2 _firePointPosition;
        private Vector2 _jointPosition;
        private Transform _transform;
        private float _elapsedTime;

        #region MonoBehaviour

        private void OnEnable()
        {
            _moveTime = 0;
            _lineRenderer.positionCount = _percision;
            _waveSize = _startWaveSize;
            _strightLine = false;

            LinePointsToFirePoint();

            _lineRenderer.enabled = true;
            _transform = transform;
            _elapsedTime = 0f;
        }

        private void OnDisable() =>
            _lineRenderer.enabled = false;

        #endregion

        private void LinePointsToFirePoint()
        {
            for (int i = 0; i < _percision; i++)
                _lineRenderer.SetPosition(i, _firePointPosition);
        }

        public bool Draw(Vector2 jointPosition)
        {
            _firePointPosition = transform.position;
            _moveTime += Time.deltaTime;
            _jointPosition = jointPosition;

            if (_strightLine == false)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= _strightDuration)
                    _strightLine = true;
                else
                    DrawRopeWaves();
            }
            else
            {
                if (_waveSize > 0)
                {
                    _waveSize -= Time.deltaTime * _straightenLineSpeed;
                    DrawRopeWaves();
                }
                else
                {
                    _waveSize = 0;
                    if (_lineRenderer.positionCount != 2)
                        _lineRenderer.positionCount = 2;

                    DrawRopeNoWaves();
                }
            }

            return _strightLine;
        }

        private void DrawRopeWaves()
        {
            for (int i = 0; i < _percision; i++)
            {
                Vector2 distance = _jointPosition - (Vector2)_transform.position;
                float delta = i / (_percision - 1f);
                Vector2 offset = Vector2.Perpendicular(distance)
                    .normalized
                        * _ropeAnimationCurve.Evaluate(delta)
                        * _waveSize;
                
                Vector2 targetPosition = Vector2.Lerp(
                    _firePointPosition,
                    _jointPosition,
                    delta) + offset;

                Vector2 currentPosition = Vector2.Lerp(
                    _firePointPosition,
                    targetPosition,
                    _ropeProgressionCurve.Evaluate(_moveTime) * _ropeProgressionSpeed);

                _lineRenderer.SetPosition(i, currentPosition);
            }
        }

        private void DrawRopeNoWaves()
        {
            _lineRenderer.SetPosition(0, _firePointPosition);
            _lineRenderer.SetPosition(1, _jointPosition);
        }
    }
}
