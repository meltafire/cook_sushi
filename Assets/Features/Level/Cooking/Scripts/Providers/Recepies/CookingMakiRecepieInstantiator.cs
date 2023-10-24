using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Sushi.Level.Cooking;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingMakiRecepieInstantiator : AssetInstantiator<ButtonView>
    {
        public CookingMakiRecepieInstantiator(IRecepieParentTransformProvider parentTransformProvider) : base(parentTransformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingMakiRecepieIngredientPrefabKey;
    }
}