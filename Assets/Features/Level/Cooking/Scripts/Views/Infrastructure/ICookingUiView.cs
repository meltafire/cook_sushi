using System;

namespace Assets.Features.Level.Cooking.Scripts.Views.Infrastructure
{
    public interface ICookingUiView
    {
        public event Action OnBackButtonClick;

        public void Toggle(bool isOn);
        public void ToggleBackButton(bool isOn);
    }
}