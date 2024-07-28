using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    public class TilemapMazeTile : MazeTileBase
    {
        public TilemapMazeTileWalls TilemapMazeTileWalls { get; }
        public Vector3Int Position { get; private set; }

        private readonly Tilemap _mazeTileTilemap;
        private readonly TileColorsSO _tileColorsSO;

        public TilemapMazeTile(
            MazeTileType state,
            Vector3Int position,
            Tilemap mazeTileTilemap,
            TilemapMazeTileWalls tilemapMazeTileWalls,
            TileColorsSO tileColorsSO
        ) : base(state, tilemapMazeTileWalls)
        {
            Position = position;
            TilemapMazeTileWalls = tilemapMazeTileWalls;

            _mazeTileTilemap = mazeTileTilemap;
            _tileColorsSO = tileColorsSO;

            // Make sure this tile has a default color
            SetColor(GetColorByState(state));

            OnStateChanged += OnStateChangedInternal;
        }

        public override void SetColor(Color color)
        {
            _mazeTileTilemap.SetColor(Position, color);
        }

        public override Color GetColor()
        {
            return _mazeTileTilemap.GetColor(Position);
        }

        private void OnStateChangedInternal(MazeTileType state)
        {
            // Make sure the color changes based on this tiles state
            SetColor(GetColorByState(state));
        }

        private Color GetColorByState(MazeTileType state)
        {
            return state switch
            {
                MazeTileType.None => _tileColorsSO.defaultColor,
                MazeTileType.Active => _tileColorsSO.activeColor,
                MazeTileType.Current => _tileColorsSO.currentColor,
                MazeTileType.Finished => _tileColorsSO.finishedColor,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}