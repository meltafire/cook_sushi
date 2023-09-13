using System;

public class LevelMenuEvents : ILevelMenuEvents, ILevelMenuExternalEvents
{
    public event Action ButtonClicked;

    public void HandleButtonClicked()
    {
        ButtonClicked?.Invoke();
    }
}
