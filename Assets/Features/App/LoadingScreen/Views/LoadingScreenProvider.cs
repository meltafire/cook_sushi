using UnityEngine;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenViewProvider : MonoBehaviour, ILoadingScreenViewProvider
    {
        [SerializeField]
        private LoadingScreenView _view;

        public LoadingScreenView View => _view;
    }
}