using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using Sushi.Level.Cooking.Events;
using Sushi.SceneReference;
using System.Threading;
using UnityEngine;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public class CookingController : Controller
    {
        private readonly ISceneReference _sceneReference;

        private CookingView _view;
        private CookingUiView _uiView;
        private UniTaskCompletionSource _completionSource;

        public CookingController(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            token.Register(OnCancellationRequested);

            await LoadPrefabs();

            ReportReady();

            await _completionSource.Task;
        }

        protected override void HandleDivingEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is ShowCookingEvent)
            {
                ShowWindow(true);
            }
        }

        private async UniTask LoadPrefabs()
        {
            await UniTask.WhenAll(LoadPrefab(), LoadUiPrefab());

            ShowWindow(false);
        }

        private void ShowWindow(bool shouldShow)
        {
            _view.Toggle(shouldShow);
            _uiView.Toggle(shouldShow);

            if (shouldShow)
            {
                _uiView.OnBackButtonClick += OnBackButtonClickHappen;
            }
            else
            {
                _uiView.OnBackButtonClick -= OnBackButtonClickHappen;
            }
        }

        private async UniTask LoadPrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(CookingConstantData.CookingPrefabKey);

            var spawnedGameObject = GameObject.Instantiate(gameObject);

            AttachResource(spawnedGameObject);

            _view = spawnedGameObject.GetComponent<CookingView>();

            assetLoader.Release();
        }

        private async UniTask LoadUiPrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(CookingConstantData.CookingUiPrefabKey);

            var spawnedGameObject = GameObject.Instantiate(gameObject, _sceneReference.OverlayCanvasTransform);

            AttachResource(spawnedGameObject);

            _uiView = spawnedGameObject.GetComponent<CookingUiView>();

            assetLoader.Release();
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }

        private void OnBackButtonClickHappen()
        {
            ShowWindow(false);
        }
    }
}