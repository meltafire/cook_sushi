using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Conveyor.Views;
using System.Threading;
using UnityEngine;
using Utils.AddressableLoader;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Controllers
{
    public class ConveyorController : Controller
    {
        private readonly ITileGameObjectData _tileGameObjectData;
        private readonly ConveyorTileControllerFactory _tileTileControllerFactory;
        private readonly ConveyorPositionService _conveyorPositionService;

        private ConveyorView _view;
        private UniTaskCompletionSource _completionSource;

        public ConveyorController(
            ITileGameObjectData tileGameObjectData,
            ConveyorTileControllerFactory tileTileControllerFactory,
            ConveyorPositionService conveyorPositionService)
        {
            _tileGameObjectData = tileGameObjectData;
            _tileTileControllerFactory = tileTileControllerFactory;
            _conveyorPositionService = conveyorPositionService;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();

            await UniTask.WhenAll(PreloadTilePrefab(), LoadConveyorPrefab());

            _conveyorPositionService.SetupConveyorData(_view.TopStart.position, _view.BottomStart.position, _view.TileCountTotal, _view.TopTileSize);

            SpawnConveyor(token);

            await _completionSource.Task;
        }

        private async UniTask PreloadTilePrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(ConveyorConstants.ConveyorTilePrefabName);

            var artLength = gameObject.GetComponent<ConveyorTileView>().SpriteLength;

            _tileGameObjectData.SetIileGameObject(gameObject, artLength);

            assetLoader.Release();
        }

        private async UniTask LoadConveyorPrefab()
        {
            var assetLoader = new AssetLoader();

            var gameObject = await assetLoader.Load(ConveyorConstants.ConveyorPrefabName);

            _view = GameObject.Instantiate(gameObject).GetComponent<ConveyorView>();

            _tileGameObjectData.TilesParentTransform = _view.TilesTransform;

            assetLoader.Release();
        }

        private void SpawnConveyor(CancellationToken token)
        {
            var count = _view.TileCountTotal;

            for (var i = 0; i < count; i++)
            {
                RunChild(_tileTileControllerFactory.Create(i), token).Forget();
            }
        }
    }
}