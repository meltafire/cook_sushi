using System;

namespace Assets.Features.Level.Conveyor.Scripts.Events
{
    public interface IConveyorTileEvents
    {
        public event Action<bool> ToggleMovementRequest;
    }
}