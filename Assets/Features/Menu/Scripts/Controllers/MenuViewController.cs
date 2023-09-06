using Cysharp.Threading.Tasks;
using Sushi.App.LoadingScreen;
using Sushi.Menu.Data;
using Sushi.Menu.Views;
using Sushi.SceneReference;
using System.Threading;
using UnityEngine;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class MenuViewController : Controller
    {
        private readonly ISceneReference _sceneReference;
        private readonly UniTaskCompletionSource _menuCompletionSource;

        private MenuView _view;

        public MenuViewController(
            ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;

            _menuCompletionSource = new UniTaskCompletionSource();
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await SpawnMenu();

            if (token.IsCancellationRequested)
            {
                return;
            }

            await HandleInput();
        }

        private async UniTask SpawnMenu()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(MenuConstants.MainMenuPrefabName);

            var spawnedGameObject = GameObject.Instantiate(gameObject, _sceneReference.OverlayCanvasTransform);

            _view = spawnedGameObject.GetComponent<MenuView>();

            AttachResource(spawnedGameObject);

            assetLoader.Release();

            RequestLoadingScreenOff();
        }

        private async UniTask HandleInput()
        {
            _view.OnButtonPressed += OnButtonPressedHappened;

            await _menuCompletionSource.Task;

            _view.OnButtonPressed -= OnButtonPressedHappened;

            RequestLoadingScreen();
        }

        private void OnButtonPressedHappened()
        {
            _menuCompletionSource.TrySetResult();
        }

        private void RequestLoadingScreen()
        {
            InvokeBubbleEvent(new LoadingScreenEvent(true));
        }

        private void RequestLoadingScreenOff()
        {
            InvokeBubbleEvent(new LoadingScreenEvent(false));
        }
    }
}