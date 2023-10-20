using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingIngridientAssetInstantiator : AssetInstantiator<IngridientButtonView>
    {
        private readonly IIngridientsParentTransformProvider _transformProvider;

        public CookingIngridientAssetInstantiator(IIngridientsParentTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public override UniTask<IngridientButtonView> Load()
        {
            return Instantiate(CookingConstantData.CookingIngredientPrefabKey, _transformProvider.Transform);
        }
    }
}