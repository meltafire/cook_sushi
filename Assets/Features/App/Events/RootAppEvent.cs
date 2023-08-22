using Sushi.App.Data;
using Utils.Controllers;

namespace Sushi.App.Events
{
    public class RootAppEvent : ControllerEvent
    {
        public readonly AppActionType AppActionType;

        public RootAppEvent(AppActionType appActionType)
        {
            AppActionType = appActionType;
        }
    }
}