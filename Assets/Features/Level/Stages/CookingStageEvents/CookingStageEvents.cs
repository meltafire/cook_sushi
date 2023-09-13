using System;

public class CookingStageEvents : ICookingStageEvents, ICookingStageExternalEvents
{
    public event Action BackButtonClicked;
    public event Action<bool> ShowRequest;

    public void ReportBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
    }

    public void RequestShow(bool isOn)
    {
        ShowRequest?.Invoke(isOn);
    }
}
