using System;

namespace Assets.Features.Menu.Scripts.Events
{
    public interface IMenuViewControllerEvents
    {
        event Action<bool> RequestEnableInput;
    }
}