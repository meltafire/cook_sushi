using Sushi.Level.Conveyor.Data;
using UnityEngine;

namespace Sushi.Level.Conveyor.Services
{
    public class ConveyorPositionService
    {
        private readonly IConveyorGameObjectData _conveyorGameObjectData;

        public ConveyorPositionService(IConveyorGameObjectData conveyorGameObjectData)
        {
            _conveyorGameObjectData = conveyorGameObjectData;
        }

        public void SetupConveyorData(float tileLength, Vector3 topStart, Vector3 bottomStart, int tileCountTotal, int topTileSize)
        {
            var topEnd = topStart + Vector3.right * tileLength * topTileSize;
            var bottomEnd = bottomStart + Vector3.right * tileLength * (tileCountTotal - topTileSize);

            _conveyorGameObjectData.SetData(
                topStart,
                topEnd,
                bottomStart,
                bottomEnd);
        }
    }
}