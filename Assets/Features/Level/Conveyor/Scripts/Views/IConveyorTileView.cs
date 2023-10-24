using System;
using UnityEngine;

namespace Assets.Features.Level.Conveyor.Scripts.Views
{
    public interface IConveyorTileView
    {
        public float SpriteLength { get; }
        public Vector3 Position { get; }


        public event Action OnUpdate;

        public void SetPosition(Vector3 position);
    }
}