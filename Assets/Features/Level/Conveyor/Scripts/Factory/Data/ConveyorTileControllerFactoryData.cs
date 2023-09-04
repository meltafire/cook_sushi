using Utils.Controllers;

namespace Sushi.Level.Conveyor.Factory.Data
{
    public class ConveyorTileControllerFactoryData : FactoryData
    {
        public readonly int TileIndex;

        public ConveyorTileControllerFactoryData(int tileIndex)
        {
            TileIndex = tileIndex;
        }
    }
}