using System;

public interface IKitchenBoardIconExternalEvents
{
    public event Action OnClick;

    public void RequestButtonToggle(bool isOn);
}
