using System;

public interface ILoadingStageControllerEvents
{
    public event Action LoadRequest;

    public void ReportLoaded();
    public void ReportStartedLoading();
}
