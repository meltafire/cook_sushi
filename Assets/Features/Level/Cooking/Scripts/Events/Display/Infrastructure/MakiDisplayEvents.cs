using System;

namespace Assets.Features.Level.Cooking.Scripts.Events.Display.Infrastructure
{
    public class MakiDisplayEvents : IMakiDisplayEvents, IMakiDisplayExternalEvents
    {
        public event Action<bool> ToggleRequest;

        public void Show(bool isOn)
        {
            ToggleRequest?.Invoke(isOn);
        }
    }
}