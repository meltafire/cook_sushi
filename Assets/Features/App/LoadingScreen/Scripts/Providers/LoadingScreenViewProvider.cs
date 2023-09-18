using UnityEngine;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenViewProvider : MonoBehaviour, ILoadingScreenViewProvider
    {
        [SerializeField]
        private Transform _transform;

        public Transform ParrentTransfrom => _transform;
    }
}