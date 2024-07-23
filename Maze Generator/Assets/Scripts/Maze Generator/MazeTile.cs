using UnityEngine;

namespace MazeGeneration
{
    public class MazeTile : MonoBehaviour
    {
        [Tooltip("Walls need to be in the order Left, Right, Up, Down")]
        [SerializeField]
        private GameObject[] _walls;

        public void ShowWall(Direction direction)
        {
            _walls[(int)direction].SetActive(false);
        }
    }
}