using Sushi.App.Data;
using System;

namespace Sushi.App.Events
{
    public class AppEventBus : IAppEventProvider, IAppMenuEventInvoker
    {
        public event Action<AppActionType> OnFeatureWorkCompletion;

        public void RequestFeatureWorkComletion()
        {
            OnFeatureWorkCompletion?.Invoke(AppActionType.Level);
        }
    }
}