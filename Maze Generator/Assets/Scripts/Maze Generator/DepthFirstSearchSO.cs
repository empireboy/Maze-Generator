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
            CoroutineRunner.Instance.StartCoroutine(Search(startTile, VisitNode));
        }

        private IEnumerator Search(MazeTile startNode, Action<MazeTile, int> visitAction = null)
        {
            Stack<MazeTile> currentSearchTiles = new();
            HashSet<MazeTile> finishedSearchTiles = new();

            currentSearchTiles.Push(startNode);

            visitAction(startNode, 2);

            while (currentSearchTiles.Count > 0)
            {
                yield return new WaitForSeconds(0.02f);

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
                    Direction nextDirectionTest = (Direction)possibleNeighbourDirections[UnityEngine.Random.Range(0, possibleNeighbourDirections.Count)];
                    MazeTile nextNode = startNode.Neighbours[(int)nextDirectionTest];

                    startNode.ShowWall(false, nextDirectionTest);

                    Debug.Log("Next Direction: " + nextDirectionTest);

                    switch (nextDirectionTest)
                    {
                        case Direction.Left:
                            nextNode.ShowWall(false, Direction.Right);
                            break;
                        case Direction.Right:
                            nextNode.ShowWall(false, Direction.Left);
                            break;
                        case Direction.Up:
                            nextNode.ShowWall(false, Direction.Down);
                            break;
                        case Direction.Down:
                            nextNode.ShowWall(false, Direction.Up);
                            break;
                    }

                    VisitNode(currentSearchTiles.Peek(), 0);

                    currentSearchTiles.Push(nextNode);

                    VisitNode(nextNode, 2);

                    startNode = nextNode;
                }
                else
                {
                    MazeTile finishedNode = currentSearchTiles.Pop();

                    finishedSearchTiles.Add(finishedNode);

                    if (currentSearchTiles.Count > 0)
                        startNode = currentSearchTiles.Peek();

                    VisitNode(finishedNode, 1);
                }
            }

            yield return null;
        }

        private bool IsValidNeighbour(List<MazeTile> neighbours, Direction direction)
        {
            return neighbours[(int)direction];
        }

        private void VisitNode(MazeTile node, int state)
        {
            switch (state)
            {
                case 0:
                    node.SetFloorColor(Color.red);
                    break;

                case 1:
                    node.SetFloorColor(Color.blue);
                    break;

                case 2:
                    node.SetFloorColor(Color.green);
                    break;
            }
        }
    }
}