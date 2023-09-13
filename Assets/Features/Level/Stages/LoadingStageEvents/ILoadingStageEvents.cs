using System;

public interface ILoadingStageEvents
{
    public void RequesLoad();

    public event Action FeatureStartedLoading;
    public event Action FeatureLoaded;
}
