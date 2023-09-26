using System;

public interface ILevelMenuExternalEvents
{
    public event Action OnClick;

    public void RequestButtonToggle(bool isOn);
}
