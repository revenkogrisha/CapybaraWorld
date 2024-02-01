using UnityEngine;

namespace Core.Other
{
    public class WorldCanvasSetup : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        private void Start() => 
            _canvas.worldCamera = Camera.main;
    }
}