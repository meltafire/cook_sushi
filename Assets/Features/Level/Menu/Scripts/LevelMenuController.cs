using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using Sushi.Level.Menu.Data;
using Sushi.SceneReference;
using System.Threading;
using UnityEngine;
using Utils.AddressableLoader;
using Utils.Controllers;

namespace Sushi.Level.Menu.Controllers
{
    public class LevelMenuController : Controller
    {
        private readonly ISceneReference _sceneReference;

        private LevelMenuView _view;
        private UniTaskCompletionSource _completionSource;

        public LevelMenuController(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();

            token.Register(OnCanceltaionRequested);

            await LoadConveyorPrefab();

            SubscribeToView();

            ReportReady();

            await _completionSource.Task;

            UnsubscribeFromView();
        }

        private async UniTask LoadConveyorPrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(LevelMenuConstants.LevelMenuPrefabName);

            var spawnedGameObject = GameObject.Instantiate(gameObject, _sceneReference.OverlayCanvasTransform);

            AttachResource(spawnedGameObject);

            _view = spawnedGameObject.GetComponent<LevelMenuView>();

            assetLoader.Release();
        }

        private void SubscribeToView()
        {
            _view.OnButtonClick += OnButtonClickHappened;
        }

        private void UnsubscribeFromView()
        {
            _view.OnButtonClick -= OnButtonClickHappened;
        }

        private void OnButtonClickHappened()
        {
            _completionSource.TrySetResult();
        }

        private void OnCanceltaionRequested()
        {
            _completionSource.TrySetResult();
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }
    }
}
