using Assets.Features.GameData.Scripts.Data;

namespace Assets.Features.Level.Cooking.Scripts.Events.Ingridients
{
    public interface IRecipeSelectionEvents
    {
        public void ReportSchemeChosen(DishType dishType);
    }
}