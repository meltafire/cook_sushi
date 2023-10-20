using System;

namespace Assets.Features.Menu.Scripts.Events
{
    public interface IMenuControllerExternalEvents
    {
        event Action ButtonClicked;
    }
}