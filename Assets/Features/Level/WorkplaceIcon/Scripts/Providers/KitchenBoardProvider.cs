using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardProvider : AssetInstantiator<KitchenBoardView>
    {
        private readonly IStageRootParentTransformProvider _transformProvider;

        public KitchenBoardProvider(IStageRootParentTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public override UniTask<KitchenBoardView> Load()
        {
            return Instantiate(KitchenBoardData.KitchenBoardPrefabKey, _transformProvider.ParentTransform);
        }
    }
}