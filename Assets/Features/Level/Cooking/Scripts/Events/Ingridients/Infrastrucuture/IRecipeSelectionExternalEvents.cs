using Assets.Features.GameData.Scripts.Data;
using System;

namespace Assets.Features.Level.Cooking.Scripts.Views.Infrastructure
{
    public interface IRecipeSelectionExternalEvents
    {
        public event Action<DishType> SchemeChosen;
    }
}