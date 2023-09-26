using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Factory.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Conveyor.Views;
using System.Threading;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Controllers
{
    public class ConveyorController : ResourcefulController
    {
        private readonly ConveyorProvider _conveyorProvider;
        private readonly ITileGameObjectData _tileGameObjectData;
        private readonly IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> _tileTileControllerFactory;
        private readonly ConveyorPositionService _conveyorPositionService;

        private ConveyorView _view;
        private ConveyorTileController[] _conveyorTiles;

        public ConveyorController(
            ConveyorProvider conveyorProvider,
            ITileGameObjectData tileGameObjectData,
            IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> tileTileControllerFactory,
            ConveyorPositionService conveyorPositionService)
        {
            _conveyorProvider = conveyorProvider;
            _tileGameObjectData = tileGameObjectData;
            _tileTileControllerFactory = tileTileControllerFactory;
            _conveyorPositionService = conveyorPositionService;
        }

        public override UniTask Initialzie(CancellationToken token)
        {
            return Load(token);
        }

        public override void Dispose()
        {
            foreach(var tile in _conveyorTiles)
            {
                tile.Dispose();
            }

            base.Dispose();
        }

        private async UniTask Load(CancellationToken token)
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

            AttachResource(assetLoader);
        }

        private async UniTask LoadConveyorPrefab()
        {
            AttachResource(_conveyorProvider);

            _view = await _conveyorProvider.Instantiate();

            _tileGameObjectData.TilesParentTransform = _view.TilesTransform;
        }

        private UniTask SpawnConveyor(CancellationToken token)
        {
            var count = _view.TileCountTotal;
            _conveyorTiles = new ConveyorTileController[count];
            var initiAlizationTasks = new UniTask[count];

            for (var i = 0; i < count; i++)
            {
                var conveyorTile = _conveyorTiles[i];

                conveyorTile = _tileTileControllerFactory.Create(new ConveyorTileControllerFactoryData(i));

                initiAlizationTasks[i] = conveyorTile.Initialzie(token);
            }

            return UniTask.WhenAll(initiAlizationTasks);
        }
    }
}