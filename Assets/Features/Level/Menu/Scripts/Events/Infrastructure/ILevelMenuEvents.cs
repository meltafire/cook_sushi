using System;

public interface ILevelMenuEvents
{
    public event Action<bool> ToggleRequest;
    public void ReportButtonClick();
}
