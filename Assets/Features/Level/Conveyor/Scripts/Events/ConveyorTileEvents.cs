using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Features.Level.Conveyor.Scripts.Events
{
    public class ConveyorTileEvents : IConveyorTileEvents, IConveyorTileExternalEvents
    {
        public event Action<bool> ToggleMovementRequest;

        public void RequestToggleMovement(bool isOn)
        {
            ToggleMovementRequest?.Invoke(isOn);
        }
    }
}