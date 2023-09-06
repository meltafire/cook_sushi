using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using System.Threading;
using UnityEngine;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Sushi.Level.Workplace
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

            await _completionSource.Task;
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

            ReportReady();

            assetLoader.Release();
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }
    }
}