using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Sushi.Level.Cooking;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayMakiStartInstantiator : AssetInstantiator<CookingDisplayRecepieView>
    {
        public CookingDisplayMakiStartInstantiator(IIngridientsDisplayParentTransformProvider parentTransformProvider) : base(parentTransformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingDisplayMakiStartPrefabKey;
    }
}