using System;

public interface IKitchenBoardIconEvents
{
    public event Action<bool> ToggleRequest;
    public void ReportButtonClick();
}
