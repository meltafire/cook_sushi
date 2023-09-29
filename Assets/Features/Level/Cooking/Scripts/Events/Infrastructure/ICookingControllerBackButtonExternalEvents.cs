using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerBackButtonExternalEvents
    {
        public event Action<bool> ToggleBackButton;

        public void RequestToggleBackButton(bool isOn);
    }
}