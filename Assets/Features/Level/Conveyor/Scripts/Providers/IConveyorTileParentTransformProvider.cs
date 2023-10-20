using UnityEngine;

namespace Assets.Features.Level.Conveyor.Scripts.Providers
{
    public interface IConveyorTileParentTransformProvider
    {
        public Transform ParentTransform { get; }
    }
}