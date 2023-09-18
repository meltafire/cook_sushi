using System;

public interface ILoadingScreenEvents
{
    public event Action<bool> ShowRequested;
}
