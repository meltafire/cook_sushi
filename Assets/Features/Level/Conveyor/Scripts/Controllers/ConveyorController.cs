using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Factory.Data;
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
        private readonly IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> _tileTileControllerFactory;
        private readonly ConveyorPositionService _conveyorPositionService;

        private ConveyorView _view;

        public ConveyorController(
            ITileGameObjectData tileGameObjectData,
            IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> tileTileControllerFactory,
            ConveyorPositionService conveyorPositionService)
        {
            _tileGameObjectData = tileGameObjectData;
            _tileTileControllerFactory = tileTileControllerFactory;
            _conveyorPositionService = conveyorPositionService;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            await UniTask.WhenAll(PreloadTilePrefab(), LoadConveyorPrefab());

            _conveyorPositionService.SetupConveyorData(_view.TopStart.position, _view.BottomStart.position, _view.TileCountTotal, _view.TopTileSize);

            await SpawnConveyor(token);
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

            var spawnedGameObject = GameObject.Instantiate(gameObject);

            AttachResource(spawnedGameObject);

            _view = spawnedGameObject.GetComponent<ConveyorView>();

            _tileGameObjectData.TilesParentTransform = _view.TilesTransform;

            assetLoader.Release();
        }

        private UniTask SpawnConveyor(CancellationToken token)
        {
            var count = _view.TileCountTotal;
            var tileTasks = new UniTask[count];

            for (var i = 0; i < count; i++)
            {
                tileTasks[i] = RunChildFromFactory(
                    _tileTileControllerFactory,
                    new ConveyorTileControllerFactoryData(i),
                    token);
            }

            ReportReady();

            return UniTask.WhenAll(tileTasks);
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }
    }
}