using System;
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

                    MazeTile mazeTile = Instantiate(mazeTilePrefab, rootTransform);

                    mazeTile.transform.localPosition = tilePosition;

                    // Initialize the Neighbours for the Left Right Up Down position
                    mazeTile.Neighbours = new() { null, null, null, null };

                    mazeTiles[x, y] = mazeTile;
                }
            }

            UpdateTileNeighbours(width, height, mazeTiles);

            return mazeTiles;
        }

        private void UpdateTileNeighbours(int width, int height, MazeTile[,] mazeTiles)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MazeTile mazeTile = mazeTiles[x, y];

                    // Associate each neighbour from the current tile
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        // Get the offset for the current direction which is the offset from the current tile to the neighbour
                        (int offsetX, int offsetY) = DirectionHelper.GetOffset(direction);

                        int neighbourX = x + offsetX;
                        int neighbourY = y + offsetY;

                        // Get the neighbour based on the direction
                        MazeTile neighbour = GetTileAt(mazeTiles, neighbourX, neighbourY, width, height);

                        if (neighbour)
                        {
                            Direction oppositeDirection = DirectionHelper.GetOppositeDirection(direction);

                            // Assign the neighbour to the current tile
                            mazeTile.Neighbours[(int)direction] = neighbour;

                            // Assign the current tile to the neighbour
                            neighbour.Neighbours[(int)oppositeDirection] = mazeTile;
                        }
                    }
                }
            }
        }

        private MazeTile GetTileAt(MazeTile[,] mazeTiles, int x, int y, int width, int height)
        {
            bool isTileInBounds = x >= 0 && y >= 0 && x < width && y < height;

            return isTileInBounds ? mazeTiles[x, y] : null;
        }

        private void ValidateGenerate(Transform rootTransform)
        {
            if (!rootTransform)
                throw new System.NullReferenceException(nameof(rootTransform));
        }
    }
}
