using System;

namespace Assets.Features.Menu.Scripts.Events
{
    public class MenuControllerEvents : IMenuControllerEvents, IMenuControllerExternalEvents
    {
        public event Action ButtonClicked;

        public void ReportButtonPressed()
        {
            ButtonClicked?.Invoke();
        }
    }
}