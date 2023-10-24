using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Sushi.Level.Cooking;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingMakiWrapActionInstantiator : AssetInstantiator<ButtonView>
    {
        public CookingMakiWrapActionInstantiator(IIngridientsDisplayParentTransformProvider parentTransformProvider) : base(parentTransformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingMakiWrapPrefabKey;
    }
}