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

        public TilemapMazeTile(
            MazeTileType state,
            Vector3Int position,
            Tilemap mazeTileTilemap,
            TilemapMazeTileWalls tilemapMazeTileWalls
        ) : base(state, tilemapMazeTileWalls)
        {
            Position = position;
            TilemapMazeTileWalls = tilemapMazeTileWalls;

            _mazeTileTilemap = mazeTileTilemap;

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
                MazeTileType.None => Color.white,
                MazeTileType.Active => Color.red,
                MazeTileType.Current => Color.green,
                MazeTileType.Finished => Color.blue,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}