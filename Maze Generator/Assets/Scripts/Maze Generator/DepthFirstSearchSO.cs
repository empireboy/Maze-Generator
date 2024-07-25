using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Depth-First Search", menuName = "MazeGeneration/Depth-First Search")]
    public class DepthFirstSearchSO : ScriptableObject
    {
        public void Search(MazeTile startTile, float timeBetweenTiles, Action searchFinishedAction)
        {
            ValidateSearch(timeBetweenTiles);

            CoroutineRunner.Instance.StartCoroutine(SearchRoutine(startTile, timeBetweenTiles, OnVisitNode, searchFinishedAction));
        }

        private IEnumerator SearchRoutine(MazeTile startNode, float timeBetweenTiles = 0, Action<MazeTile, MazeTileType> visitAction = null, Action searchFinishedAction = null)
        {
            Stack<MazeTile> currentSearchTiles = new();
            HashSet<MazeTile> finishedSearchTiles = new();

            currentSearchTiles.Push(startNode);

            visitAction?.Invoke(startNode, MazeTileType.Current);

            while (currentSearchTiles.Count > 0)
            {
                if (timeBetweenTiles > 0)
                    yield return new WaitForSeconds(timeBetweenTiles);

                List<int> possibleNeighbourDirections = new();

                // Go through all the neighbours to get all possible directions for the search 
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    MazeTile neighbour = startNode.Neighbours[(int)direction];

                    if (!neighbour)
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
                    MazeTile nextNode = startNode.Neighbours[(int)nextDirection];

                    startNode.ShowWall(false, nextDirection);
                    nextNode.ShowWall(false, DirectionHelper.GetOppositeDirection(nextDirection));

                    visitAction?.Invoke(currentSearchTiles.Peek(), MazeTileType.Active);

                    currentSearchTiles.Push(nextNode);

                    visitAction?.Invoke(nextNode, MazeTileType.Current);

                    startNode = nextNode;
                }
                else
                {
                    MazeTile finishedNode = currentSearchTiles.Pop();

                    finishedSearchTiles.Add(finishedNode);

                    if (currentSearchTiles.Count > 0)
                        startNode = currentSearchTiles.Peek();

                    visitAction?.Invoke(finishedNode, MazeTileType.Finished);
                }
            }

            searchFinishedAction?.Invoke();

            yield return null;
        }

        private bool IsValidNeighbour(List<MazeTile> neighbours, Direction direction)
        {
            return neighbours[(int)direction];
        }

        private void OnVisitNode(MazeTile node, MazeTileType state)
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