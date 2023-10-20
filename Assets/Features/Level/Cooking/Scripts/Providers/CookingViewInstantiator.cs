using Assets.Features.Level.Cooking.Scripts.Providers;
using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.Level.Cooking
{
    public class CookingViewInstantiator : AssetInstantiator<CookingView>
    {
        private readonly ICookingViewParentTransformProvider _parentTransformProvider;

        public CookingViewInstantiator(ICookingViewParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public override UniTask<CookingView> Load()
        {
            return Instantiate(CookingConstantData.CookingPrefabKey, _parentTransformProvider.Transform);
        }
    }
}