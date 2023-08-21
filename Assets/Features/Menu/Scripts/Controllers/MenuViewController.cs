using Cysharp.Threading.Tasks;
using Sushi.Menu.Data;
using Sushi.Menu.Views;
using Sushi.SceneReference;
using System.Threading;
using UnityEngine;
using Utils.AddressableLoader;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class MenuViewController : Controller
    {
        private readonly AssetLoader _assetLoader;
        private readonly ISceneReference _sceneReference;

        private MenuView _view;
        private UniTaskCompletionSource _menuCompletionSource;

        public MenuViewController(
            AssetLoader assetLoader,
            ISceneReference sceneReference)
        {
            _assetLoader = assetLoader;
            _sceneReference = sceneReference;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await SpawnMenu();

            if (token.IsCancellationRequested)
            {
                return;
            }

            await HandleInput();

            ReleaseMenu();
        }

        private async UniTask SpawnMenu()
        {
            var gameObject = await _assetLoader.Load(MenuConstants.MainMenuPrefabName);

            _view = GameObject.Instantiate(gameObject, _sceneReference.OverlayCanvasTransform).GetComponent<MenuView>();

            _assetLoader.Release();
        }

        private async UniTask HandleInput()
        {
            _menuCompletionSource = new UniTaskCompletionSource();

            _view.OnButtonPressed += OnButtonPressedHappened;

            await _menuCompletionSource.Task;

            _view.OnButtonPressed -= OnButtonPressedHappened;
        }

        private void ReleaseMenu()
        {
            _view.Release();
        }

        private void OnButtonPressedHappened()
        {
            _menuCompletionSource.TrySetResult();
        }
    }
}