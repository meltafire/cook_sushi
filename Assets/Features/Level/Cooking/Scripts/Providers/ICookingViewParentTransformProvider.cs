using UnityEditor;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Providers
{
    public interface ICookingViewParentTransformProvider
    {
        public Transform Transform { get; }
    }
}