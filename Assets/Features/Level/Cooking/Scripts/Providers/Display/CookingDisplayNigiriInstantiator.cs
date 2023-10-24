using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Sushi.Level.Cooking;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayNigiriInstantiator : AssetInstantiator<CookingDisplayRecepieView>
    {
        public CookingDisplayNigiriInstantiator(IIngridientsDisplayParentTransformProvider transformProvider) : base(transformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingDisplayNigiriPrefabKey;
    }
}