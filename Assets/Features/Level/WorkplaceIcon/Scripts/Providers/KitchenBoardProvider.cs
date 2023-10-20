using Assets.Features.Level.WorkplaceIcon.Scripts.Providers;
using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardProvider : AssetInstantiator<KitchenBoardView>
    {
        private readonly IKitchenBoardButtonTransformProvider _transformProvider;

        public KitchenBoardProvider(IKitchenBoardButtonTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public override UniTask<KitchenBoardView> Load()
        {
            return Instantiate(KitchenBoardData.KitchenBoardPrefabKey, _transformProvider.ParrentTransform);
        }
    }
}