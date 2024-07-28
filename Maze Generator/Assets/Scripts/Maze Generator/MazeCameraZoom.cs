using UnityEngine;

namespace MazeGeneration
{
    public class MazeCameraZoom : MonoBehaviour
    {
        [SerializeField]
        private float _extraBorderWidth = 5;

        [SerializeField]
        private Camera _mazeCamera;

        [SerializeField]
        private MazeGenerator _mazeGenerator;

        private void Awake()
        {
            _mazeGenerator.OnGenerateMaze += OnGenerateMaze;
        }

        private void OnGenerateMaze(float width, float height)
        {
            // Calculate required orthographic sizes
            float requiredHeight = height / 2 + _extraBorderWidth;
            float viewportAspect = _mazeCamera.rect.width / _mazeCamera.rect.height;
            float requiredWidth = (width / 2 + _extraBorderWidth) / viewportAspect;

            // Adjust the camera zoom to ensure the entire maze fits within the view
            _mazeCamera.orthographicSize = Mathf.Max(requiredHeight, requiredWidth);
        }
    }
}