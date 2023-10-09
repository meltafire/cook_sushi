using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public interface ICookingControllerEvents
    {
        public event Action ShowRequest;
        public event Action<bool> ToggleBackButton;
        public event Action<bool> ToggleRevertButton;
        public event Action<bool> ToggleDoneButton;

        public void ReportPopupClosed();
        public void ReportRevertPressed();
        public void ReportDonePressed();
    }
}