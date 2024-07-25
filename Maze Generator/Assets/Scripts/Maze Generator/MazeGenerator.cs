using System;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        public const int MinWidth = 10;
        public const int MaxWidth = 250;
        public const int MinHeight = 10;
        public const int MaxHeight = 250;
        public const float MinSearchDelay = 0;
        public const float MaxSearchDelay = 5;

        public Transform MazeRootTransform => _mazeRootTransform;

        // Generate maze event with Width and Height
        public Action<float, float> OnGenerateMaze;

        [SerializeField]
        private bool _autoGenerate;

        [Range(MinWidth, MaxWidth)]
        [SerializeField]
        private int _defaultWidth = 25;

        [Range(MinHeight, MaxHeight)]
        [SerializeField]
        private int _defaultHeight = 25;

        [SerializeField]
        private float _defaultSearchTimeBetweenTiles = 0.05f;

        [SerializeField]
        private MazeGeneratorSO _mazeGeneratorSO;

        [SerializeField]
        private Transform _mazeRootTransform;

        private void Start()
        {
            if (_autoGenerate)
                Generate(_defaultWidth, _defaultHeight, _defaultSearchTimeBetweenTiles, _mazeRootTransform);
        }

        public void Generate(int width, int height, float searchTimeBetweenTiles, Transform rootTransform)
        {
            ValidateGenerate(width, height);

            OnGenerateMaze?.Invoke(width, height);

            _mazeGeneratorSO.Generate(width, height, searchTimeBetweenTiles, rootTransform);
        }

        private void ValidateGenerate(int width, int height)
        {
            // Make sure the width is within range
            if (width < MinWidth || width > MaxWidth)
                throw new ArgumentOutOfRangeException(nameof(width), width, null);

            // Make sure the height is within range
            if (height < MinHeight || height > MaxHeight)
                throw new ArgumentOutOfRangeException(nameof(height), height, null);

            if (!_mazeGeneratorSO)
                throw new ArgumentNullException(nameof(_mazeGeneratorSO));
        }
    }
}
