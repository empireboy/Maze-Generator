using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeTileNode : MazeTileBase, INode<MazeTileNode>
    {
        public List<MazeTileNode> Neighbours { get; set; }

        private readonly SpriteRenderer _spriteRenderer;

        public MazeTileNode(
            MazeTileType state,
            SpriteRenderer spriteRenderer,
            GameObjectMazeTileWalls gameObjectMazeTileWalls
        ) : base(state, gameObjectMazeTileWalls)
        {
            _spriteRenderer = spriteRenderer;
        }

        public override void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public override Color GetColor()
        {
            return _spriteRenderer.color;
        }
    }
}