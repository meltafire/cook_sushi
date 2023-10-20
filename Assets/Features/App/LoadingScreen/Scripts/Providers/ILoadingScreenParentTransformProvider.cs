using UnityEngine;

namespace Sushi.App.LoadingScreen
{
    public interface ILoadingScreenParentTransformProvider
    {
        public Transform ParentTransform { get; }
    }
}