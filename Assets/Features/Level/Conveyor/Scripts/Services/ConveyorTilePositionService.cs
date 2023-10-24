using Sushi.Level.Conveyor.Data;
using UnityEngine;

namespace Sushi.Level.Conveyor.Services
{
    public class ConveyorTilePositionService
    {
        private readonly IConveyorPointsProvider _pointProvider;

        public ConveyorTilePositionService(IConveyorPointsProvider pointProvider)
        {
            _pointProvider = pointProvider;
        }

        public bool IsTileOnTopRow(int positionNumber)
        {
            var tilePositionForTopRow = GenerateTilePositionForTopRow(positionNumber);

            return ShouldTileBeInTopRow(tilePositionForTopRow);
        }

        public Vector3 GetPosition(int positionNumber)
        {
            var tilePositionForTopRow = GenerateTilePositionForTopRow(positionNumber);

            var shouldTileBePlacedInTop = ShouldTileBeInTopRow(tilePositionForTopRow);

            if (shouldTileBePlacedInTop)
            {
                return new Vector3(tilePositionForTopRow, _pointProvider.TopStart.y, _pointProvider.TopStart.z);
            }
            else
            {
                var tilesLengthOutOfRangeOfTopRow = tilePositionForTopRow - _pointProvider.TopEnd.x;
                var tilesOutOfRangeOfTopRow = tilesLengthOutOfRangeOfTopRow / _dimensionProvider.TileLength;

                var localTilePosition = _dimensionProvider.TileLength * tilesOutOfRangeOfTopRow;
                var tilePositionForBottomRow = _pointProvider.BottomStart.x + localTilePosition;

                return new Vector3(tilePositionForBottomRow, _pointProvider.BottomStart.y, _pointProvider.BottomStart.z); 
            }
        }

        private bool ShouldTileBeInTopRow(float tilePositionForTopRow)
        {
            return tilePositionForTopRow < _pointProvider.TopEnd.x;
        }

        private float GenerateTilePositionForTopRow(int positionNumber)
        {
            var localTilePosition = _dimensionProvider.TileLength * positionNumber;
            var tilePositionForTopRow = _pointProvider.TopStart.x + localTilePosition;

            return tilePositionForTopRow;
        }
    }
}