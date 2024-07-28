using System;
using UnityEngine;

namespace MazeGeneration
{
    public abstract class MazeTileBase : IColorable
    {
        protected MazeTileType state;
        public MazeTileType State
        {
            get => state;
            set
            {
                if (value == state)
                    return;

                state = value;

                OnStateChanged?.Invoke(state);
            }
        }

        public MazeTileWallsBase Walls { get; private set; }

        public event Action<MazeTileType> OnStateChanged;

        public MazeTileBase(MazeTileType state, MazeTileWallsBase mazeTileWalls)
        {
            State = state;
            Walls = mazeTileWalls;
        }

        public abstract void SetColor(Color color);
        public abstract Color GetColor();
    }
}