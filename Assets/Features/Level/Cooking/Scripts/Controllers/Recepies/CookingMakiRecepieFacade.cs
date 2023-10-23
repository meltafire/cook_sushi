using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Reflex.Core;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public class CookingMakiRecepieFacade : UniqueVcFacade<CookingMakiRecepieController, IngridientButtonView>
    {
        protected override string ContainerName => ContainerNameInner;

        private static readonly string ContainerNameInner = "CookingMakiRecepieFacade";

        public CookingMakiRecepieFacade(
            AssetInstantiator<IngridientButtonView> assetInstantiator,
            Container container)
            : base(assetInstantiator, container)
        {
        }
    }
}