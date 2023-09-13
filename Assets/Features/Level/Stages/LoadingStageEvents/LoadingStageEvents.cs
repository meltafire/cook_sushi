using System;

public class LoadingStageEvents : ILoadingStageEvents, ILoadingStageControllerEvents
{
    public event Action LoadRequest;
    public event Action FeatureLoaded;
    public event Action FeatureStartedLoading;

    public void ReportStartedLoading()
    {
        FeatureStartedLoading?.Invoke();
    }

    public void ReportLoaded()
    {
        FeatureLoaded?.Invoke();
    }

    public void RequesLoad()
    {
        LoadRequest?.Invoke();
    }
}
