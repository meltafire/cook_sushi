using System;

public class IdleStageEvents : IIdleStageEvents, IIdleStageControllerEvents
{
    public event Action LaunchGameplayRequest;

    public void LaunchGameplay()
    {
        LaunchGameplayRequest?.Invoke();
    }
}
