using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public class CookingMakiRecepieController : CookingRecepieController
    {
        public CookingMakiRecepieController(
            ButtonView view,
            IRecipeSelectionEvents events)
             : base(view, events)
        {
        }

        protected override DishType ReportData()
        {
            return DishType.Maki;
        }
    }
}