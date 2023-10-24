using Sushi.Level.Conveyor.Views;
using Utils.AssetProvider;

namespace Assets.Features.Level.Conveyor.Scripts.Providers
{
    public class ConveyorTileInstantiator : LocalAssetInstantiator<ConveyorTileView>
    {
        public ConveyorTileInstantiator(
            BaseConveyorTileProvider gameObjectProvider,
            ISceneRenderReference transformProvider)
            : base(gameObjectProvider, transformProvider)
        {
        }
    }
}