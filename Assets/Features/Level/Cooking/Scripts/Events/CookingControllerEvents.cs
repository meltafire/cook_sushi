using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public class CookingControllerEvents :
        ICookingControllerEvents,
        ICookingControllerExternalEvents,
        ICookingControllerBackButtonExternalEvents,
        ICookingControllerRecepieButtonsExternalEvents
    {
        public event Action ShowRequest;
        public event Action<bool> ToggleBackButton;
        public event Action<bool> ToggleRevertButton;
        public event Action<bool> ToggleDoneButton;
        public event Action PopupClosed;
        public event Action RevertPressed;
        public event Action DonePressed;

        public void RequestShow()
        {
            ShowRequest?.Invoke();
        }

        public void ReportPopupClosed()
        {
            PopupClosed?.Invoke();
        }

        public void RequestToggleBackButton(bool isOn)
        {
            ToggleBackButton?.Invoke(isOn);
        }

        public void ReportRevertPressed()
        {
            RevertPressed?.Invoke();
        }

        public void ReportDonePressed()
        {
            DonePressed?.Invoke();
        }

        public void ToggleRevert(bool isOn)
        {
            ToggleRevertButton?.Invoke(isOn);
        }

        public void ToggleDone(bool isOn)
        {
            ToggleDoneButton?.Invoke(isOn);
        }
    }
}