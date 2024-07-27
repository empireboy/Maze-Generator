using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Depth-First Search", menuName = "MazeGeneration/Depth-First Search")]
    public class DepthFirstSearchSO : ScriptableObject
    {
        public void Search(MazeTileNode startTile, float timeBetweenTiles, Action searchFinishedAction)
        {
            ValidateSearch(timeBetweenTiles);

            CoroutineRunner.Instance.StartCoroutine(SearchRoutine(startTile, timeBetweenTiles, OnVisitNode, searchFinishedAction));
        }

        private IEnumerator SearchRoutine(
            MazeTileNode startNode,
            float timeBetweenTiles = 0,
            Action<MazeTileNode, MazeTileType> visitAction = null,
            Action searchFinishedAction = null
        )
        {
            Stack<MazeTileNode> currentSearchTiles = new();
            Stack<MazeTileNode> finishedSearchTiles = new();

            currentSearchTiles.Push(startNode);

            visitAction?.Invoke(startNode, MazeTileType.Current);

            while (currentSearchTiles.Count > 0)
            {
                // Delay the search algorithm to visualize it
                if (timeBetweenTiles > 0)
                    yield return new WaitForSeconds(timeBetweenTiles);

                List<int> possibleNeighbourDirections = new();

                // Go through all the neighbours to get all possible directions for the search 
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    MazeTileNode neighbour = startNode.Neighbours[(int)direction];

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
                    MazeTileNode nextNode = startNode.Neighbours[(int)nextDirection];

                    startNode.Walls.ShowWall(false, nextDirection);
                    nextNode.Walls.ShowWall(false, DirectionHelper.GetOppositeDirection(nextDirection));

                    visitAction?.Invoke(currentSearchTiles.Peek(), MazeTileType.Active);

                    currentSearchTiles.Push(nextNode);

                    visitAction?.Invoke(nextNode, MazeTileType.Current);

                    // Make sure to remove the current state from the last finished tile
                    // since it is not being searched currently
                    if (finishedSearchTiles.Count > 0)
                    {
                        MazeTileNode lastFinishedMazeTile = finishedSearchTiles.Peek();

                        if (lastFinishedMazeTile.State == MazeTileType.Current)
                            visitAction?.Invoke(lastFinishedMazeTile, MazeTileType.Finished);
                    }

                    startNode = nextNode;
                }
                else
                {
                    if (finishedSearchTiles.Count > 0)
                        visitAction?.Invoke(finishedSearchTiles.Peek(), MazeTileType.Finished);

                    MazeTileNode finishedNode = currentSearchTiles.Pop();

                    finishedSearchTiles.Push(finishedNode);

                    if (currentSearchTiles.Count > 0)
                        startNode = currentSearchTiles.Peek();

                    visitAction?.Invoke(finishedNode, MazeTileType.Current);
                }
            }

            searchFinishedAction?.Invoke();

            yield return null;
        }

        private void OnVisitNode(MazeTileNode node, MazeTileType state)
        {
            node.State = state;
        }

        private void ValidateSearch(float timeBetweenTiles)
        {
            if (timeBetweenTiles < 0)
                throw new ArgumentOutOfRangeException(nameof(timeBetweenTiles), timeBetweenTiles, null);
        }
    }
}