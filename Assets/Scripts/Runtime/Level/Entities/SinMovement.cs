using UnityEngine;

namespace Core.Level
{
	public class SinMovement : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private SinMovementConfig _config;
		
		private float _birthTime;
		private float _startY;

		#region MonoBehaviour

		private void Awake()
		{
			_birthTime = Time.time;
			_startY = transform.position.y;
			Invoke(nameof(EndLifecycle), _config.Lifetime);
		}

		private void FixedUpdate() => 
			Move();
			
		#endregion

		private void Move()
		{
			Vector3 movement = Vector2.zero;
			float age = Time.time - _birthTime;
			float theta = Mathf.PI * 2 * age / _config.WaveFriquency;
			float sinTheta = Mathf.Sin(theta);

			movement.y = _startY + _config.WaveWidth * sinTheta - 1f;
			movement.x = _config.Speed;

			_rigidbody2D.velocity = movement;
		}
		
		private void EndLifecycle() =>
			Destroy(gameObject);
	}
}
