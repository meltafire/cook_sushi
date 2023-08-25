using Sushi.Level.Conveyor.Data;
using UnityEngine;

namespace Sushi.Level.Conveyor.Services
{
    public class ConveyorPositionService
    {
        private readonly IConveyorGameObjectData _conveyorGameObjectData;
        private readonly ITileGameObjectDimensionProvider _dimensionProvider;

        public ConveyorPositionService(IConveyorGameObjectData conveyorGameObjectData, ITileGameObjectDimensionProvider dimensionProvider)
        {
            _conveyorGameObjectData = conveyorGameObjectData;
            _dimensionProvider = dimensionProvider;
        }

        public void SetupConveyorData(Vector3 topStart, Vector3 bottomStart, int tileCountTotal, int topTileSize)
        {
            var topEnd = topStart + Vector3.right * _dimensionProvider.TileLength * topTileSize;
            var bottomEnd = bottomStart + Vector3.right * _dimensionProvider.TileLength * (tileCountTotal - topTileSize);

            _conveyorGameObjectData.SetData(
                topStart,
                topEnd,
                bottomStart,
                bottomEnd);
        }
    }
}