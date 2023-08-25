using UnityEngine;

namespace Sushi.Level.Conveyor.Data
{
    public interface IConveyorPointProvider
    {
        public Vector3 TopStart { get; }
        public Vector3 TopEnd { get; }
        public Vector3 BottomStart { get; }
        public Vector3 BottomEnd { get; }
    }
}