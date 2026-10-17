using GameFrame.Core.Definitions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Definitions
{
    public class MapDefinition : BaseDefinition
    {
        public Vector2 Size { get; set; }
        public int[,] TextureMap { get; set; } 
    }
}
