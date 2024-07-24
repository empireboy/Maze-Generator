using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Depth-First Search", menuName = "MazeGeneration/Depth-First Search")]
    public class DepthFirstSearchSO : ScriptableObject
    {
        public void Search(MazeTile startTile)
        {
            CoroutineRunner.Instance.StartCoroutine(SearchRoutine(startTile, VisitNode));
        }

        private IEnumerator SearchRoutine(MazeTile startNode, Action<MazeTile, MazeTileType> visitAction = null)
        {
            Stack<MazeTile> currentSearchTiles = new();
            HashSet<MazeTile> finishedSearchTiles = new();

            currentSearchTiles.Push(startNode);

            visitAction(startNode, MazeTileType.Current);

            while (currentSearchTiles.Count > 0)
            {
                yield return new WaitForSeconds(0.05f);

                List<int> possibleNeighbourDirections = new();

                if (
                    IsValidNeighbour(startNode.Neighbours, Direction.Left) &&
                    !finishedSearchTiles.Contains(startNode.Neighbours[(int)Direction.Left]) &&
                    !currentSearchTiles.Contains(startNode.Neighbours[(int)Direction.Left])
                )
                {
                    possibleNeighbourDirections.Add((int)Direction.Left);
                }

                if (
                    IsValidNeighbour(startNode.Neighbours, Direction.Right) &&
                    !finishedSearchTiles.Contains(startNode.Neighbours[(int)Direction.Right]) &&
                    !currentSearchTiles.Contains(startNode.Neighbours[(int)Direction.Right])
                )
                {
                    possibleNeighbourDirections.Add((int)Direction.Right);
                }

                if (
                    IsValidNeighbour(startNode.Neighbours, Direction.Up) &&
                    !finishedSearchTiles.Contains(startNode.Neighbours[(int)Direction.Up]) &&
                    !currentSearchTiles.Contains(startNode.Neighbours[(int)Direction.Up])
                )
                {
                    possibleNeighbourDirections.Add((int)Direction.Up);
                }

                if (
                    IsValidNeighbour(startNode.Neighbours, Direction.Down) &&
                    !finishedSearchTiles.Contains(startNode.Neighbours[(int)Direction.Down]) &&
                    !currentSearchTiles.Contains(startNode.Neighbours[(int)Direction.Down])
                )
                {
                    possibleNeighbourDirections.Add((int)Direction.Down);
                }

                if (possibleNeighbourDirections.Count > 0)
                {
                    Direction nextDirection = (Direction)possibleNeighbourDirections[UnityEngine.Random.Range(0, possibleNeighbourDirections.Count)];
                    MazeTile nextNode = startNode.Neighbours[(int)nextDirection];

                    startNode.ShowWall(false, nextDirection);
                    nextNode.ShowWall(false, DirectionHelper.GetOppositeDirection(nextDirection));

                    visitAction(currentSearchTiles.Peek(), MazeTileType.Active);

                    currentSearchTiles.Push(nextNode);

                    visitAction(nextNode, MazeTileType.Current);

                    startNode = nextNode;
                }
                else
                {
                    MazeTile finishedNode = currentSearchTiles.Pop();

                    finishedSearchTiles.Add(finishedNode);

                    if (currentSearchTiles.Count > 0)
                        startNode = currentSearchTiles.Peek();

                    visitAction(finishedNode, MazeTileType.Finished);
                }
            }

            yield return null;
        }

        private bool IsValidNeighbour(List<MazeTile> neighbours, Direction direction)
        {
            return neighbours[(int)direction];
        }

        private void VisitNode(MazeTile node, MazeTileType state)
        {
            node.State = state;
        }
    }
}