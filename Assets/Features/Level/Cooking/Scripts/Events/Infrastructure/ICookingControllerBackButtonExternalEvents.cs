using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerBackButtonExternalEvents
    {
        public void RequestToggleBackButton(bool isOn);
    }
}