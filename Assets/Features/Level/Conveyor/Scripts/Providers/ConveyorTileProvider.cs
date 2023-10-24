using Assets.Utils.AssetProvider;
using Sushi.Level.Conveyor.Data;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Conveyor.Scripts.Providers
{
    public abstract class BaseConveyorTileProvider : GameObjectProvider
    {
        protected BaseConveyorTileProvider(AssetLoader assetLoader) : base(assetLoader)
        {
        }
    }


    public class ConveyorTileProvider : BaseConveyorTileProvider
    {
        protected override string AssetName => ConveyorConstants.ConveyorTilePrefabName;

        public ConveyorTileProvider(AssetLoader assetLoader) : base(assetLoader)
        {
        }
    }
}