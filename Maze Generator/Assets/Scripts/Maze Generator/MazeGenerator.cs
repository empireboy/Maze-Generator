using UnityEngine;

namespace MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        public const int MinWidth = 10;
        public const int MinHeight = 10;

        [SerializeField]
        private bool _autoGenerate;

        [Range(10, 250)]
        [SerializeField]
        private int _defaultWidth = 25;

        [Range(10, 250)]
        [SerializeField]
        private int _defaultHeight = 25;

        [SerializeField]
        private MazeGeneratorSO _mazeGeneratorSO;

        private void Start()
        {
            if (_autoGenerate)
                Generate(_defaultWidth, _defaultHeight, transform);
        }

        public void Generate(int width, int height, Transform rootTransform)
        {
            if (!_mazeGeneratorSO)
                throw new System.NullReferenceException(nameof(_mazeGeneratorSO));

            if (!rootTransform)
                throw new System.NullReferenceException(nameof(rootTransform));

            _mazeGeneratorSO.Generate(width, height, rootTransform);
        }
    }
}
