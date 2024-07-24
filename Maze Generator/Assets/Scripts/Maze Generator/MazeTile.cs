using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeTile : MonoBehaviour, INode<MazeTile>
    {
        public List<MazeTile> Neighbours { get; set; }

        [Tooltip("Walls need to be in the order Left, Right, Up, Down")]
        [SerializeField]
        private GameObject[] _walls;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public void ShowWall(bool active, Direction direction)
        {
            _walls[(int)direction].SetActive(active);
        }

        public void SetFloorColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}