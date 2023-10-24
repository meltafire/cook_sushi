using UnityEngine;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenParentTransformProvider : MonoBehaviour, ILoadingScreenParentTransformProvider
    {
        [SerializeField]
        private Transform _transform;

        public Transform Transform => _transform;
    }
}