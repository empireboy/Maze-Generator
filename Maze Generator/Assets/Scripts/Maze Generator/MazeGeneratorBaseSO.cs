using System;
using UnityEngine;

namespace MazeGeneration
{
    public abstract class MazeGeneratorBaseSO<T> : ScriptableObject where T : MazeTileBase
    {
        [SerializeField]
        private TilemapDepthFirstSearchSO _depthFirstSearch;

        public void Generate(int width, int height, float searchTimeBetweenTiles, Transform rootTransform)
        {
            ValidateGenerate(rootTransform);

            ClearRootObject(rootTransform);

            // Create the grid and start the search algorithm when it finishes
            CreateMazeGrid(
                width,
                height,
                rootTransform,
                mazeTiles => OnCreateMazeGridFinished(mazeTiles, searchTimeBetweenTiles)
            );
        }

        protected abstract void CreateMazeGrid(int width, int height, Transform rootTransform, Action<T[,]> finishedAction);

        protected void ClearRootObject(Transform rootTransform)
        {
            foreach (Transform childObject in rootTransform)
            {
                Destroy(childObject.gameObject);
            }
        }

        protected void ResetTileStates(T[,] mazeTiles)
        {
            for (int x = 0; x < mazeTiles.GetLength(0); x++)
            {
                for (int y = 0; y < mazeTiles.GetLength(1); y++)
                {
                    mazeTiles[x, y].State = MazeTileType.None;
                }
            }
        }

        protected T GetTileAt(T[,] mazeTiles, int x, int y, int width, int height)
        {
            bool isTileInBounds = x >= 0 && y >= 0 && x < width && y < height;

            return isTileInBounds ? mazeTiles[x, y] : null;
        }

        protected virtual void OnSearchFinished(T[,] mazeTiles)
        {
            ResetTileStates(mazeTiles);
        }

        protected virtual void OnCreateMazeGridFinished(T[,] mazeTiles, float searchTimeBetweenTiles)
        {
            // Start the search algorithm and cleanup after it is finished
            _depthFirstSearch.Search(mazeTiles, mazeTiles[0, 0], searchTimeBetweenTiles, () => OnSearchFinished(mazeTiles));
        }

        protected virtual void ValidateGenerate(Transform rootTransform)
        {
            if (!rootTransform)
                throw new NullReferenceException(nameof(rootTransform));

            if (!_depthFirstSearch)
                throw new NullReferenceException(nameof(_depthFirstSearch));
        }
    }
}
