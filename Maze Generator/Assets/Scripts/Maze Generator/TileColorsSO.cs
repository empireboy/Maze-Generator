using UnityEngine;

namespace MazeGeneration
{
    [CreateAssetMenu(fileName = "Tile Colors", menuName = "MazeGeneration/Tile Colors")]
    public class TileColorsSO : ScriptableObject
    {
        public Color defaultColor;
        public Color activeColor;
        public Color currentColor;
        public Color finishedColor;
    }
}
