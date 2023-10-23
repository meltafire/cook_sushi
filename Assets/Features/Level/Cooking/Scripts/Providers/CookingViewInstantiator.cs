using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.Level.Cooking
{
    public class CookingViewInstantiator : AssetInstantiator<CookingView>
    {
        private readonly IStageRootParentTransformProvider _parentTransformProvider;

        public CookingViewInstantiator(IStageRootParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public override UniTask<CookingView> Load()
        {
            return Instantiate(CookingConstantData.CookingPrefabKey, _parentTransformProvider.ParentTransform);
        }
    }
}