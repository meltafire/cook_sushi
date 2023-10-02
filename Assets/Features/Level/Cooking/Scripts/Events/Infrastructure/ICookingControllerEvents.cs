using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerEvents
    {
        public event Action ShowRequest;
        public event Action<bool> ToggleBackButton;

        public void ReportPopupClosed();
    }
}