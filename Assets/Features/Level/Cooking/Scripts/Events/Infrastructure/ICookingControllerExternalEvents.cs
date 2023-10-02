using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerExternalEvents
    {
        public void RequestShow();

        public event Action PopupClosed;
    }
}