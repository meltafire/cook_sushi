using System;

public class KitchenBoardIconEvents : IKitchenBoardIconEvents, IKitchenBoardIconExternalEvents
{
    public event Action OnClick;
    public event Action<bool> ToggleRequest;

    public void ReportButtonClick()
    {
        OnClick?.Invoke();
    }

    public void RequestButtonToggle(bool isOn)
    {
        ToggleRequest?.Invoke(isOn);
    }
}
