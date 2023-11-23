using UnityEngine;

namespace Core.Level
{
	[CreateAssetMenu(fileName = "Sin Movement Config", menuName = "Configs/Sin Movement Config")]
	public class SinMovementConfig : ScriptableObject
	{
		[SerializeField] private float _speed = -12;
		
		[Tooltip("Lower value increases the friquency")]
		[SerializeField, Range(0f, 10f)] private float _waveFriquency = 4f;
		[SerializeField, Min(0f)] private float _waveWidth = 5f;
		
		[Space]
		[SerializeField, Min(0f)] private float _lifetime = 40f;

		public float Speed => _speed;
		public float WaveFriquency => _waveFriquency;
		public float WaveWidth => _waveWidth;
		public float Lifetime => _lifetime;
	}
}