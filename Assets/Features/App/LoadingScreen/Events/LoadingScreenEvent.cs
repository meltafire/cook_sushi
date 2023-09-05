using Utils.Controllers;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenEvent : ControllerEvent
    {
        public readonly bool ShouldShow;

        public LoadingScreenEvent(bool shouldShow)
        {
            ShouldShow = shouldShow;
        }
    }
}