using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Maze Generator", menuName = "MazeGeneration/Maze Generator")]
    public class MazeGeneratorSO : ScriptableObject
    {
        public MazeTile mazeTilePrefab;

        [SerializeField]
        private DepthFirstSearchSO _depthFirstSearch;

        public void Generate(int width, int height, Transform rootTransform)
        {
            ValidateGenerate(rootTransform);

            MazeTile[,] mazeTiles = CreateMazeGrid(width, height, rootTransform);

            _depthFirstSearch.Search(mazeTiles[0, 0]);
        }

        private MazeTile[,] CreateMazeGrid(int width, int height, Transform rootTransform)
        {
            MazeTile[,] mazeTiles = new MazeTile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Make sure the maze is centered
                    Vector2 tilePosition = new(x - width / 2, y - height / 2);

                    MazeTile mazeTile = Instantiate(mazeTilePrefab, tilePosition, Quaternion.identity, rootTransform);

                    // Initialize the Neighbours for the Left Right Up Down position
                    mazeTile.Neighbours = new List<MazeTile>() { null, null, null, null };

                    mazeTiles[x, y] = mazeTile;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MazeTile mazeTile = mazeTiles[x, y];

                    // Left neighbour
                    if (x > 0)
                    {
                        mazeTile.Neighbours[(int)Direction.Left] = mazeTiles[x - 1, y];
                        mazeTiles[x - 1, y].Neighbours[(int)Direction.Right] = mazeTile;
                    }

                    // Right neighbour
                    if (x < width - 1)
                    {
                        mazeTile.Neighbours[(int)Direction.Right] = mazeTiles[x + 1, y];
                        mazeTiles[x + 1, y].Neighbours[(int)Direction.Left] = mazeTile;
                    }

                    // Bottom neighbour
                    if (y > 0)
                    {
                        mazeTile.Neighbours[(int)Direction.Down] = mazeTiles[x, y - 1];
                        mazeTiles[x, y - 1].Neighbours[(int)Direction.Up] = mazeTile;
                    }

                    // Top neighbour
                    if (y < height - 1)
                    {
                        mazeTile.Neighbours[(int)Direction.Up] = mazeTiles[x, y + 1];
                        mazeTiles[x, y + 1].Neighbours[(int)Direction.Down] = mazeTile;
                    }
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
