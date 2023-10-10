using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public class CookingNigiriRecepieController : CookingRecepieController
    {
        public CookingNigiriRecepieController(
            IRecepieParentTransformProvider parentTransformProvider,
            CookingNigiriRecepieAssetProvider ingridientProvider,
            IRecipeSelectionEvents events)
             : base(parentTransformProvider, ingridientProvider, events)
        {
        }

        protected override DishType ReportData()
        {
            return DishType.Nigiri;
        }
    }
}