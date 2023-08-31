using Cysharp.Threading.Tasks;
using Sushi.Level.Menu.Data;
using Sushi.SceneReference;
using System;
using System.Threading;
using UnityEngine;
using Utils.AddressableLoader;
using Utils.Controllers;

namespace Sushi.Level.Menu.Controllers
{
    public class LevelMenuController : Controller, IDisposable
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
            await LoadConveyorPrefab();

            SubscribeToView();

            _completionSource = new UniTaskCompletionSource();

            await _completionSource.Task.AttachExternalCancellation(token);

            UnsubscribeFromView();
        }

        private async UniTask LoadConveyorPrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(LevelMenuConstants.LevelMenuPrefabName);

            _view = GameObject.Instantiate(gameObject, _sceneReference.OverlayCanvasTransform).GetComponent<LevelMenuView>();

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

        public void Dispose()
        {
            Debug.Log("LevelMenuController , done");
        }
    }
}
