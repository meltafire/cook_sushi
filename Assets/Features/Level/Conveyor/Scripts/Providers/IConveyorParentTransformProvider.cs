using UnityEngine;

namespace Assets.Features.Level.Conveyor.Scripts.Providers
{
    public interface IConveyorParentTransformProvider
    {
        public Transform ParentTransform { get; }
    }
}