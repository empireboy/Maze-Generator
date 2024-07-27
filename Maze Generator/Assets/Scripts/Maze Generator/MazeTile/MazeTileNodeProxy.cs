using System;
using System.Linq;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeTileNodeProxy : MonoBehaviour
    {
        private MazeTileNode _mazeTile;
        public MazeTileNode MazeTile => _mazeTile;

        [SerializeField]
        private GameObject[] _walls;

        [SerializeField]
        private SpriteRenderer _floorSpriteRenderer;

        private void Awake()
        {
            GameObjectMazeTileWalls mazeTileWalls = new(_walls.ToList());

            // Create the tile instance and add the walls to it
            _mazeTile = new(MazeTileType.None, _floorSpriteRenderer, mazeTileWalls);

            // Make sure the color changes when the state of this tile changes
            _mazeTile.OnStateChanged += SetColorByState;
        }

        private void OnDestroy()
        {
            _mazeTile.OnStateChanged -= SetColorByState;
        }

        private void SetColorByState(MazeTileType mazeTileType)
        {
            _mazeTile.SetColor(GetColorByState(mazeTileType));
        }

        private Color GetColorByState(MazeTileType mazeTileType)
        {
            return mazeTileType switch
            {
                MazeTileType.None => Color.white,
                MazeTileType.Active => Color.red,
                MazeTileType.Current => Color.green,
                MazeTileType.Finished => Color.blue,
                _ => throw new ArgumentOutOfRangeException(nameof(mazeTileType), mazeTileType, null)
            };
        }
    }
}