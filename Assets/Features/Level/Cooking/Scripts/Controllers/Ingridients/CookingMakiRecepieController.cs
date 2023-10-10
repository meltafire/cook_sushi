using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public class CookingMakiRecepieController : CookingRecepieController
    {
        public CookingMakiRecepieController(
            IRecepieParentTransformProvider parentTransformProvider,
            CookingMakiRecepieAssetProvider ingridientProvider,
            IRecipeSelectionEvents events)
             : base(parentTransformProvider, ingridientProvider, events)
        {
        }

        protected override DishType ReportData()
        {
            return DishType.Maki;
        }
    }
}