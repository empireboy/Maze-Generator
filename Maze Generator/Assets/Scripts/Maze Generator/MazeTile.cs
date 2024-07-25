using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeTile : MonoBehaviour, INode<MazeTile>
    {
        private MazeTileType _state;
        public MazeTileType State
        {
            get => _state;
            set
            {
                if (value == _state)
                    return;

                _state = value;

                OnStateChanged?.Invoke(_state);
            }
        }

        public List<MazeTile> Neighbours { get; set; }
        public Action<MazeTileType> OnStateChanged;

        [Tooltip("Walls need to be in the order Left, Right, Up, Down")]
        [SerializeField]
        private GameObject[] _walls;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            // Make sure the color changes when the state changes
            OnStateChanged += SetColorByState;
        }

        private void OnDestroy()
        {
            OnStateChanged -= SetColorByState;
        }

        public void ShowWall(bool active, Direction direction)
        {
            _walls[(int)direction].SetActive(active);
        }

        private void SetColorByState(MazeTileType mazeTileType)
        {
            _spriteRenderer.color = GetColorByState(mazeTileType);
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