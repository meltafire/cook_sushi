using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Reflex.Core;
using Utils.AssetProvider;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public class CookingMakiRecepieFacade : UniqueVcFacade<CookingMakiRecepieController, ButtonView>
    {
        protected override string ContainerName => ContainerNameInner;

        private static readonly string ContainerNameInner = "CookingMakiRecepieFacade";

        public CookingMakiRecepieFacade(
            AssetInstantiator<ButtonView> assetInstantiator,
            Container container)
            : base(assetInstantiator, container)
        {
        }
    }
}