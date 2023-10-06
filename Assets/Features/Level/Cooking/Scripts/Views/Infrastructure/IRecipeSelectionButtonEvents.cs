using System;

namespace Assets.Features.Level.Cooking.Scripts.Views.Infrastructure
{
    public interface IRecipeSelectionButtonEvents
    {
        public event Action<CookingScheme> SchemeChosen;

        public void ShowButtons(bool isOn);
    }
}