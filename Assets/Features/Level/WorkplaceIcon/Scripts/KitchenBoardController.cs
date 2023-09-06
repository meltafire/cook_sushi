using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using System;
using System.Threading;
using UnityEngine;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardController : Controller
    {
        private KitchenBoardView _view;
        private UniTaskCompletionSource _completionSource;

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            token.Register(OnCancellationRequested);

            await LoadConveyorPrefab();

            ReportReady();

            await _completionSource.Task;

            _view.OnClick -= OnClickHappened;
        }

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }

        private async UniTask LoadConveyorPrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(KitchenBoardData.KitchenBoardPrefabKey);

            var spawnedGameObject = GameObject.Instantiate(gameObject);

            AttachResource(spawnedGameObject);

            _view = spawnedGameObject.GetComponent<KitchenBoardView>();

            assetLoader.Release();
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }

        protected override void HandleDivingEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is GameplayLaunchEvent)
            {
                _view.OnClick += OnClickHappened;
            }
        }

        private void OnClickHappened()
        {
            throw new NotImplementedException();
        }
    }
}