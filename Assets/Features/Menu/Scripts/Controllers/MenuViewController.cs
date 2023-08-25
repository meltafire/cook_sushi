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
        private readonly ISceneReference _sceneReference;

        private MenuView _view;
        private UniTaskCompletionSource _menuCompletionSource;

        public MenuViewController(
            ISceneReference sceneReference)
        {
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
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(MenuConstants.MainMenuPrefabName);

            _view = GameObject.Instantiate(gameObject, _sceneReference.OverlayCanvasTransform).GetComponent<MenuView>();

            assetLoader.Release();
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