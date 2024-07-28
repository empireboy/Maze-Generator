using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Tilemap Maze Generator", menuName = "MazeGeneration/Tilemap Maze Generator")]
    public class TilemapMazeGeneratorSO : MazeGeneratorBaseSO<TilemapMazeTile>
    {
        public Transform floorTilemapPrefab;
        public Transform leftWallsTilemapPrefab;
        public Transform rightWallsTilemapPrefab;
        public Transform topWallsTilemapPrefab;
        public Transform bottomWallsTilemapPrefab;
        public Sprite floorSprite;
        public Sprite wallSprite;
        public int tileCreationBatchSize = 30;

        protected override void CreateMazeGrid(
            int width,
            int height,
            Transform rootTransform,
            Action<TilemapMazeTile[,]> finishedAction
        )
        {
            TilemapMazeTile[,] mazeTiles = new TilemapMazeTile[width, height];

            // Create the maze tilemap
            (Tilemap floorTilemap, Tilemap leftWallsTilemap, Tilemap rightWallsTilemap, Tilemap topWallsTilemap, Tilemap bottomWallsTilemap) = CreateMazeTilemaps(
                width,
                height,
                rootTransform
            );

            // Make sure the previous maze generation is stopped
            CoroutineRunner.Instance.StopAllCoroutines();

            CoroutineRunner.Instance.StartCoroutine(CreateMazeGridRoutine(
                width,
                height,
                floorTilemap,
                leftWallsTilemap,
                rightWallsTilemap,
                topWallsTilemap,
                bottomWallsTilemap,
                mazeTiles,
                finishedAction
            ));
        }

        private IEnumerator CreateMazeGridRoutine(
            int width,
            int height,
            Tilemap floorTilemap,
            Tilemap leftWallsTilemap,
            Tilemap rightWallsTilemap,
            Tilemap topWallsTilemap,
            Tilemap bottomWallsTilemap,
            TilemapMazeTile[,] mazeTiles,
            Action<TilemapMazeTile[,]> finishedAction
        )
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile floorTile = CreateInstance<Tile>();
                    floorTile.sprite = floorSprite;
                    floorTile.color = Color.white;

                    Vector3Int floorTilePosition = new(x, y, 0);

                    // Add all floor tiles to the tilemap
                    floorTilemap.SetTile(floorTilePosition, floorTile);

                    // Make sure you can change the color in the tiles
                    floorTilemap.SetTileFlags(floorTilePosition, TileFlags.None);

                    // Create the wall tiles which have references to their location on their specific tilemap
                    TilemapMazeTileWalls mazeTileWalls = new(
                        floorTilePosition,
                        wallSprite,
                        Color.black,
                        leftWallsTilemap,
                        rightWallsTilemap,
                        topWallsTilemap,
                        bottomWallsTilemap
                    );

                    // Create the tiles and add their respective walls
                    TilemapMazeTile mazeTile = new(MazeTileType.None, floorTilePosition, floorTilemap, mazeTileWalls, tileColorsSO);

                    // Make the walls visible
                    mazeTile.Walls.ShowWall(true, Direction.Left);
                    mazeTile.Walls.ShowWall(true, Direction.Right);
                    mazeTile.Walls.ShowWall(true, Direction.Up);
                    mazeTile.Walls.ShowWall(true, Direction.Down);

                    mazeTiles[x, y] = mazeTile;

                    // Spread out the tile creation over multiple frames
                    // Pauses the creation every x frames
                    if ((x * height + y) % tileCreationBatchSize == 0)
                        yield return null;
                }
            }

            // Remove two edge walls so that it actually is maze with a start and end
            mazeTiles[0, 0].Walls.ShowWall(false, Direction.Left);
            mazeTiles[width - 1, height - 1].Walls.ShowWall(false, Direction.Right);

            finishedAction?.Invoke(mazeTiles);
        }

        private (Tilemap floorTilemap, Tilemap leftWallsTilemap, Tilemap rightWallsTilemap, Tilemap topWallsTilemap, Tilemap bottomWallsTilemap) CreateMazeTilemaps(
            float width,
            float height,
            Transform rootTransform
        )
        {
            // Create the maze tilemap
            Transform tilemapTransform = Instantiate(floorTilemapPrefab, rootTransform);

            // Create all wall tilemaps
            Transform leftWallsTilemapTransform = Instantiate(leftWallsTilemapPrefab, rootTransform);
            Transform rightWallsTilemapTransform = Instantiate(rightWallsTilemapPrefab, rootTransform);
            Transform topWallsTilemapTransform = Instantiate(topWallsTilemapPrefab, rootTransform);
            Transform bottomWallsTilemapTransform = Instantiate(bottomWallsTilemapPrefab, rootTransform);

            // Get the tilemap components from all tilemap objects
            Tilemap tilemap = tilemapTransform.GetComponentInChildren<Tilemap>();
            Tilemap leftWallsTilemap = leftWallsTilemapTransform.GetComponentInChildren<Tilemap>();
            Tilemap rightWallsTilemap = rightWallsTilemapTransform.GetComponentInChildren<Tilemap>();
            Tilemap topWallsTilemap = topWallsTilemapTransform.GetComponentInChildren<Tilemap>();
            Tilemap bottomWallsTilemap = bottomWallsTilemapTransform.GetComponentInChildren<Tilemap>();

            // Center all the tilemaps
            float offsetX = width / 2f;
            float offsetY = height / 2f;

            tilemapTransform.localPosition = new(-offsetX, -offsetY, 0);
            leftWallsTilemapTransform.localPosition = new(-offsetX, -offsetY, 0);
            rightWallsTilemapTransform.localPosition = new(-offsetX, -offsetY, 0);
            topWallsTilemapTransform.localPosition = new(-offsetX, -offsetY, 0);
            bottomWallsTilemapTransform.localPosition = new(-offsetX, -offsetY, 0);

            return (tilemap, leftWallsTilemap, rightWallsTilemap, topWallsTilemap, bottomWallsTilemap);
        }

        protected override void ValidateGenerate(Transform rootTransform)
        {
            base.ValidateGenerate(rootTransform);

            if (tileCreationBatchSize <= 0)
                throw new ArgumentException($"{nameof(tileCreationBatchSize)} must be greater than 0", nameof(tileCreationBatchSize));
        }
    }
}
