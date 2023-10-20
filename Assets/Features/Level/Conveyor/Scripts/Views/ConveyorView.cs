using Assets.Features.Level.Conveyor.Scripts.Views;
using UnityEngine;

namespace Sushi.Level.Conveyor.Views
{
    public class ConveyorView : MonoBehaviour, IConveyorView
    {
        [SerializeField]
        private int _tileCountTotal;
        [SerializeField]
        private int _topTileSize;
        [SerializeField]
        private Transform _tilesTransform;
        [SerializeField]
        private Transform _topStart;
        [SerializeField]
        private Transform _bottomStart;

        public Transform TilesTransform => _tilesTransform;
        public Transform TopStart => _topStart;
        public Transform BottomStart => _bottomStart;
        public int TileCountTotal => _tileCountTotal;
        public int TopTileSize => _topTileSize;
    }
}