using System;

namespace Assets.Features.Level.Cooking.Scripts.Events.Infrastructure
{
    public interface ICookingControllerRecepieButtonsExternalEvents
    {
        public event Action RevertPressed;
        public event Action DonePressed;

        public void ToggleRevert(bool isOn);
        public void ToggleDone(bool isOn);
    }
}