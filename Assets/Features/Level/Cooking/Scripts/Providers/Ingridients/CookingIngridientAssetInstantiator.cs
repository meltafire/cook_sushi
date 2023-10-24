using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Sushi.Level.Cooking;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingIngridientAssetInstantiator : AssetInstantiator<IngridientButtonView>
    {
        public CookingIngridientAssetInstantiator(IIngridientsParentTransformProvider transformProvider) : base(transformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingIngredientPrefabKey;
    }
}