using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    public class TilemapMazeTile : MazeTileBase, IColorable
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
        }

        public override void SetColor(Color color)
        {
            _mazeTileTilemap.SetColor(Position, color);
        }

        public override Color GetColor()
        {
            return _mazeTileTilemap.GetColor(Position);
        }
    }
}