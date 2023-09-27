using System;

namespace Assets.Features.Level.Cooking.Scripts.Events
{
    public class CookingControllerEvents : ICookingControllerEvents, ICookingControllerExternalEvents
    {
        public event Action<bool> ShowRequest;
        public event Action BackButtonClicked;

        public void ReportBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }

        public void RequestShow(bool toggle)
        {
            ShowRequest?.Invoke(toggle);
        }
    }
}