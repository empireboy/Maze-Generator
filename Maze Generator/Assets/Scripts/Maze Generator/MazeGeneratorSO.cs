using System;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Maze Generator", menuName = "MazeGeneration/Maze Generator")]
    public class MazeGeneratorSO : MazeGeneratorBaseSO<MazeTileNode>
    {
        public MazeTileNodeProxy mazeTilePrefab;

        protected override void CreateMazeGrid(
            int width,
            int height,
            Transform rootTransform,
            Action<MazeTileNode[,]> finishedAction
        )
        {
            MazeTileNode[,] mazeTiles = new MazeTileNode[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Make sure the maze is centered
                    float offsetX = (width - 1f) / 2f;
                    float offsetY = (height - 1f) / 2f;

                    Vector2 tilePosition = new(x - offsetX, y - offsetY);

                    MazeTileNodeProxy mazeTileProxy = Instantiate(mazeTilePrefab, rootTransform);

                    mazeTileProxy.transform.localPosition = tilePosition;

                    // Initialize the Neighbours for the Left Right Up Down position
                    mazeTileProxy.MazeTile.Neighbours = new() { null, null, null, null };

                    mazeTiles[x, y] = mazeTileProxy.MazeTile;
                }
            }

            UpdateTileNeighbours(width, height, mazeTiles);

            finishedAction?.Invoke(mazeTiles);
        }

        private void UpdateTileNeighbours(int width, int height, MazeTileNode[,] mazeTiles)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MazeTileNode mazeTile = mazeTiles[x, y];

                    // Associate each neighbour from the current tile
                    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                    {
                        // Get the offset for the current direction which is the offset from the current tile to the neighbour
                        (int offsetX, int offsetY) = DirectionHelper.GetOffset(direction);

                        int neighbourX = x + offsetX;
                        int neighbourY = y + offsetY;

                        // Get the neighbour based on the direction
                        MazeTileNode neighbour = GetTileAt(mazeTiles, neighbourX, neighbourY, width, height);

                        if (neighbour != null)
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
    }
}
