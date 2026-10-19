using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Models
{
    public class Map
    {
        public Vector2Int Size { get; set; }
        public int[,] TextureMap { get; set; } 
    }
}
