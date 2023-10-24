namespace Sushi.Level.Conveyor.Data
{
    public class ConveyorTileData
    {
        public bool IsTopRow;
        public int Index;

        public ConveyorTileData(bool isTopRow, int index)
        {
            IsTopRow = isTopRow;
            Index = index;
        }
    }
}