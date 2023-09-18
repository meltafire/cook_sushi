using System;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenEvents : ILoadingScreenExternalEvents, ILoadingScreenEvents
    {
        public event Action<bool> ShowRequested;

        public void Show(bool isOn)
        {
            ShowRequested?.Invoke(isOn);
        }
    }
}