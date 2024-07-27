using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    public class TilemapMazeTileWalls : MazeTileWallsBase
    {
        public Vector3Int Position { get; private set; }
        public Sprite WallSprite { get; private set; }
        public Color WallColor { get; private set; }

        private readonly Tilemap _leftWallsTilemap;
        private readonly Tilemap _rightWallsTilemap;
        private readonly Tilemap _topWallsTilemap;
        private readonly Tilemap _bottomWallsTilemap;

        public TilemapMazeTileWalls(
            Vector3Int wallPosition,
            Sprite wallSprite,
            Color wallColor,
            Tilemap leftWallsTilemap,
            Tilemap rightWallsTilemap,
            Tilemap topWallsTilemap,
            Tilemap bottomWallsTilemap
        )
        {
            Position = wallPosition;
            WallSprite = wallSprite;
            WallColor = wallColor;

            _leftWallsTilemap = leftWallsTilemap;
            _rightWallsTilemap = rightWallsTilemap;
            _topWallsTilemap = topWallsTilemap;
            _bottomWallsTilemap = bottomWallsTilemap;
        }

        public override void ShowWall(bool active, Direction direction)
        {
            // Add a new Tile to the Tilemap to show it
            // and assign null to hide it
            Tile wallTile = active ? CreateWallTile() : null;

            switch (direction)
            {
                case Direction.Left:

                    _leftWallsTilemap.SetTile(Position, wallTile);

                    break;

                case Direction.Right:

                    _rightWallsTilemap.SetTile(Position, wallTile);

                    break;

                case Direction.Up:

                    _topWallsTilemap.SetTile(Position, wallTile);

                    break;

                case Direction.Down:

                    _bottomWallsTilemap.SetTile(Position, wallTile);

                    break;
            }
        }

        public void SetWallsColor(Color color)
        {
            _leftWallsTilemap.SetColor(Position, color);
            _rightWallsTilemap.SetColor(Position, color);
            _topWallsTilemap.SetColor(Position, color);
            _bottomWallsTilemap.SetColor(Position, color);
        }

        private Tile CreateWallTile()
        {
            Tile wallTile = ScriptableObject.CreateInstance<Tile>();
            wallTile.sprite = WallSprite;
            wallTile.color = WallColor;

            return wallTile;
        }
    }
}