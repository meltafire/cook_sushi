using UnityEngine;

namespace Utils.AssetProvider
{
    public interface IParentTransformProvider
    {
        Transform Transform { get; }
    }
}