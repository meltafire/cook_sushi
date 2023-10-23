using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Conveyor.Scripts.Providers
{
    public class ConveyorTileInstantiator : AssetInstantiator<ConveyorTileView>
    {
        private readonly IStageRootParentTransformProvider _transformProvider;

        public ConveyorTileInstantiator(IStageRootParentTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public override UniTask<ConveyorTileView> Load()
        {
            return Instantiate(ConveyorConstants.ConveyorTilePrefabName, _transformProvider.ParentTransform);
        }
    }
}