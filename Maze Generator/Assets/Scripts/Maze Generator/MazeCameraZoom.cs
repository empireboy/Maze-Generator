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
            _mazeCamera.orthographicSize = width / 2 + _extraBorderWidth;
        }
    }
}