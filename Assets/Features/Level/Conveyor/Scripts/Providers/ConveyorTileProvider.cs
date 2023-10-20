using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Conveyor.Scripts.Providers
{
    public class ConveyorTileProvider : AssetInstantiator<ConveyorTileView>
    {
        private readonly IConveyorTileParentTransformProvider _transformProvider;

        public ConveyorTileProvider(IConveyorTileParentTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public override UniTask<ConveyorTileView> Load()
        {
            return Instantiate(ConveyorConstants.ConveyorTilePrefabName, _transformProvider.ParentTransform);
        }
    }
}