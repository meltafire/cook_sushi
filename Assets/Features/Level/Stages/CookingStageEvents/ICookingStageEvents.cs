using System;

public interface ICookingStageEvents
{
    public event Action BackButtonClicked;

    public void RequestShow(bool isOn);
}
