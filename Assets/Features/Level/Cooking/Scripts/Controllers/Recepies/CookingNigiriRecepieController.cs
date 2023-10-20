using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public class CookingNigiriRecepieController : CookingRecepieController
    {
        public CookingNigiriRecepieController(
            ButtonView view,
            IRecipeSelectionEvents events)
             : base(view, events)
        {
        }

        protected override DishType ReportData()
        {
            return DishType.Nigiri;
        }
    }
}