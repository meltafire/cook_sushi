using UnityEngine;

namespace Assets.Features.Level.Conveyor.Scripts.Views
{
    public interface IConveyorView
    {
        public Transform TilesTransform { get; }
        public Transform TopStart { get; }
        public Transform BottomStart { get; }
        public int TileCountTotal { get; }
        public int TopTileSize { get; }
    }
}