using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerEvents
    {
        public event Action ShowRequest;

        public void ReportPopupClosed();
    }
}