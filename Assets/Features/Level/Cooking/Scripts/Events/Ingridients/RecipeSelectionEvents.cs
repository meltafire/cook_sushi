using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using System;
using UnityEditor;

namespace Assets.Features.Level.Cooking.Scripts.Events.Ingridients
{
    public class RecipeSelectionEvents : IRecipeSelectionEvents, IRecipeSelectionExternalEvents
    {
        public event Action<DishType> SchemeChosen;

        public void ReportSchemeChosen(DishType dishType)
        {
            SchemeChosen?.Invoke(dishType);
        }
    }
}