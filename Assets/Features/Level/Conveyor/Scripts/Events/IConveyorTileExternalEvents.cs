using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Features.Level.Conveyor.Scripts.Events
{
    public interface IConveyorTileExternalEvents
    {
        public void RequestToggleMovement(bool isOn);
    }
}