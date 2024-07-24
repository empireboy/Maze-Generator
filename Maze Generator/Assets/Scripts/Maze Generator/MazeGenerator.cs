using UnityEngine;

namespace MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        public const int MinWidth = 10;
        public const int MaxWidth = 250;
        public const int MinHeight = 10;
        public const int MaxHeight = 250;

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

        private void Start()
        {
            if (_autoGenerate)
                Generate(_defaultWidth, _defaultHeight, _defaultSearchTimeBetweenTiles, transform);
        }

        public void Generate(int width, int height, float searchTimeBetweenTiles, Transform rootTransform)
        {
            ValidateGenerate(width, height, rootTransform);

            _mazeGeneratorSO.Generate(width, height, searchTimeBetweenTiles, rootTransform);
        }

        private void ValidateGenerate(int width, int height, Transform rootTransform)
        {
            // Make sure the width is within range
            if (width < MinWidth || width > MaxWidth)
                throw new System.ArgumentOutOfRangeException(nameof(width));

            // Make sure the height is within range
            if (height < MinHeight || height > MaxHeight)
                throw new System.ArgumentOutOfRangeException(nameof(height));

            if (!_mazeGeneratorSO)
                throw new System.ArgumentNullException(nameof(_mazeGeneratorSO));
        }
    }
}
