using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Sushi.Level.Cooking;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayMakiWrapInstantiator : AssetInstantiator<CookingDisplayRecepieView>
    {
        public CookingDisplayMakiWrapInstantiator(IIngridientsDisplayParentTransformProvider parentTransformProvider) : base(parentTransformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingDisplayMakiWrapPrefabKey;
    }
}