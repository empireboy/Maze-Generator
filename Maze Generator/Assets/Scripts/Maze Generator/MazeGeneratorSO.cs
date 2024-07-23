using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Maze Generator", menuName = "MazeGeneration/Maze Generator")]
    public class MazeGeneratorSO : ScriptableObject
    {
        public MazeTile mazeTilePrefab;

        public void Generate(int width, int height, Transform rootTransform)
        {
            ValidateGenerate(rootTransform);

            List<MazeTile> mazeTiles = CreateMazeGrid(width, height, rootTransform);
        }

        private List<MazeTile> CreateMazeGrid(int width, int height, Transform rootTransform)
        {
            List<MazeTile> mazeTiles = new();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Make sure the maze is centered
                    Vector2 tilePosition = new(x - width / 2, y - height / 2);

                    MazeTile mazeTile = Instantiate(mazeTilePrefab, tilePosition, Quaternion.identity, rootTransform);

                    mazeTiles.Add(mazeTile);
                }
            }

            return mazeTiles;
        }

        private void ValidateGenerate(Transform rootTransform)
        {
            if (!rootTransform)
                throw new System.NullReferenceException(nameof(rootTransform));
        }
    }
}
