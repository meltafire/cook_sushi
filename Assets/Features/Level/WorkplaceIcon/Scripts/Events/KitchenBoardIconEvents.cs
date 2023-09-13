using System;

public class KitchenBoardIconEvents : IKitchenBoardIconEvents, IKitchenBoardIconExternalEvents
{
    public event Action OnClick;

    public void ReportButtonClick()
    {
        OnClick?.Invoke();
    }
}
