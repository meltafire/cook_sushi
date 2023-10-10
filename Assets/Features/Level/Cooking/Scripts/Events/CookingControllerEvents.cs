using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public class CookingControllerEvents :
        ICookingControllerEvents,
        ICookingControllerExternalEvents
    {
        public event Action ShowRequest;
        public event Action PopupClosed;

        public void RequestShow()
        {
            ShowRequest?.Invoke();
        }

        public void ReportPopupClosed()
        {
            PopupClosed?.Invoke();
        }
    }
}