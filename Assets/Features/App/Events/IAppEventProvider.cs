using Sushi.App.Data;
using System;

namespace Sushi.App.Events
{
    public interface IAppEventProvider
    {
        public event Action<AppActionType> OnFeatureWorkCompletion;
    }
}