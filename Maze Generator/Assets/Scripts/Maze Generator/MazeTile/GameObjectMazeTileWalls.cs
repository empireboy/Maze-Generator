using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class GameObjectMazeTileWalls : MazeTileWallsBase
    {
        private readonly List<GameObject> _wallObjects;

        public GameObjectMazeTileWalls(List<GameObject> wallObjects)
        {
            _wallObjects = wallObjects;
        }

        public override void ShowWall(bool active, Direction direction)
        {
            GameObject wallObject = _wallObjects[(int)direction];

            if (!wallObject)
                return;

            wallObject.SetActive(active);
        }
    }
}