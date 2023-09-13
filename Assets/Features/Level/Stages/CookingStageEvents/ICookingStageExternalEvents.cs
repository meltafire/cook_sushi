using System;

public interface ICookingStageExternalEvents
{
    public event Action<bool> ShowRequest;

    public void ReportBackButtonClicked();
}
