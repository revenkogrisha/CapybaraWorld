using UnityEngine;

namespace Core.Other
{
    public class WorldCanvasSetup : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        private void Awake() => 
            _canvas.worldCamera = Camera.main;
    }
}