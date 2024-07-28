using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Tilemap Depth-First Search", menuName = "MazeGeneration/Tilemap Depth-First Search")]
    public class TilemapDepthFirstSearchSO : ScriptableObject
    {
        public int tileSearchBatchSize = 30;

        public void Search(
            MazeTileBase[,] mazeTiles,
            MazeTileBase startTile,
            float timeBetweenTiles,
            Action searchFinishedAction
        )
        {
            ValidateSearch(timeBetweenTiles);

            // Cast the types to use the tile type that belongs to the Tilemap
            TilemapMazeTile[,] tilemapMazeTiles = mazeTiles as TilemapMazeTile[,];
            TilemapMazeTile tilemapStartTile = startTile as TilemapMazeTile;

            CoroutineRunner.Instance.StartCoroutine(SearchRoutine(
                tilemapMazeTiles,
                tilemapStartTile,
                timeBetweenTiles,
                OnVisitNode,
                searchFinishedAction
            ));
        }

        private IEnumerator SearchRoutine(
            TilemapMazeTile[,] mazeTiles,
            TilemapMazeTile startTile,
            float timeBetweenTiles = 0,
            Action<TilemapMazeTile, MazeTileType> visitAction = null,
            Action searchFinishedAction = null
        )
        {
            Stack<TilemapMazeTile> currentSearchTiles = new();
            Stack<TilemapMazeTile> finishedSearchTiles = new();

            currentSearchTiles.Push(startTile);

            visitAction?.Invoke(startTile, MazeTileType.Current);

            // Keep track of the iteration of the search algorithm
            // This is used for batching tile searching
            int iteration = 0;

            while (currentSearchTiles.Count > 0)
            {
                // Delay the search algorithm to visualize it
                if (timeBetweenTiles > 0)
                    yield return new WaitForSeconds(timeBetweenTiles);

                List<int> possibleNeighbourDirections = new();

                // Go through all the neighbours to get all possible directions for the search 
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    TilemapMazeTile neighbour = GetMazeTileNeighbour(mazeTiles, startTile.Position, direction);

                    if (neighbour == null)
                        continue;

                    // Add the direction if the current neighbour is not finished for the search
                    // and is not currently in the path being searched
                    if (
                        !finishedSearchTiles.Contains(neighbour) &&
                        !currentSearchTiles.Contains(neighbour)
                    )
                        possibleNeighbourDirections.Add((int)direction);
                }

                if (possibleNeighbourDirections.Count > 0)
                {
                    Direction nextDirection = (Direction)possibleNeighbourDirections[UnityEngine.Random.Range(0, possibleNeighbourDirections.Count)];
                    TilemapMazeTile nextNode = GetMazeTileNeighbour(mazeTiles, startTile.Position, nextDirection);

                    startTile.Walls.ShowWall(false, nextDirection);
                    nextNode.Walls.ShowWall(false, DirectionHelper.GetOppositeDirection(nextDirection));

                    visitAction?.Invoke(currentSearchTiles.Peek(), MazeTileType.Active);

                    currentSearchTiles.Push(nextNode);

                    visitAction?.Invoke(nextNode, MazeTileType.Current);

                    // Make sure to remove the current state from the last finished tile
                    // since it is not being searched currently
                    if (finishedSearchTiles.Count > 0)
                    {
                        TilemapMazeTile lastFinishedMazeTile = finishedSearchTiles.Peek();

                        if (lastFinishedMazeTile.State == MazeTileType.Current)
                            visitAction?.Invoke(lastFinishedMazeTile, MazeTileType.Finished);
                    }

                    startTile = nextNode;
                }
                else
                {
                    if (finishedSearchTiles.Count > 0)
                        visitAction?.Invoke(finishedSearchTiles.Peek(), MazeTileType.Finished);

                    TilemapMazeTile finishedNode = currentSearchTiles.Pop();

                    finishedSearchTiles.Push(finishedNode);

                    if (currentSearchTiles.Count > 0)
                        startTile = currentSearchTiles.Peek();

                    visitAction?.Invoke(finishedNode, MazeTileType.Current);
                }

                // Make sure to delay the search when the search delay is not being used
                // This will make sure the application doesn't freeze by spreading the workload
                if (timeBetweenTiles <= 0)
                    if (iteration % tileSearchBatchSize == 0)
                        yield return null;

                iteration++;
            }

            searchFinishedAction?.Invoke();
        }

        private TilemapMazeTile GetMazeTileNeighbour(TilemapMazeTile[,] mazeTiles, Vector3Int position, Direction direction)
        {
            (int offsetX, int offsetY) = DirectionHelper.GetOffset(direction);

            Vector3Int neighbourPosition = new(position.x + offsetX, position.y + offsetY, position.z);

            // Make sure the neighbour is in range of the grid
            if (
                neighbourPosition.x >= 0 && neighbourPosition.x < mazeTiles.GetLength(0) &&
                neighbourPosition.y >= 0 && neighbourPosition.y < mazeTiles.GetLength(1)
            )
                return mazeTiles[neighbourPosition.x, neighbourPosition.y];

            return null;
        }

        private void OnVisitNode(TilemapMazeTile node, MazeTileType state)
        {
            node.State = state;
        }

        private void ValidateSearch(float timeBetweenTiles)
        {
            if (timeBetweenTiles < 0)
                throw new ArgumentOutOfRangeException(nameof(timeBetweenTiles), timeBetweenTiles, null);

            if (tileSearchBatchSize <= 0)
                throw new ArgumentException($"{nameof(tileSearchBatchSize)} must be greater than 0", nameof(tileSearchBatchSize));
        }
    }
}
