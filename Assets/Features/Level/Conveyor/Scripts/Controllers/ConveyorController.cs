using Assets.Features.Level.Conveyor.Scripts.Controllers;
using Assets.Features.Level.Conveyor.Scripts.Views;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Controllers
{
    public abstract class BaseConveyorController : BaseConveyorTileHolder
    {
        protected BaseConveyorController(Container container) : base(container)
        {
        }
    }

    public class ConveyorController : BaseConveyorController
    {
        private static readonly string ContainerName = "ConveyorController";

        private readonly IConveyorView _view;
        private readonly Queue<IController> _tileHolders = new Queue<IController>();

        public ConveyorController(IConveyorView view, Container container) : base(container)
        {
            _view = view;
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            return SpawnConveyor(token);
        }

        protected override void ActAfterContainerDisposed()
        {
        }

        protected override void ActBeforeContainerDisposed()
        {
            while (_tileHolders.Count != 0)
            {
                var controller = _tileHolders.Dequeue();

                controller.Dispose();
            }
        }

        protected override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            return UniTask.FromResult(
                Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddTransient(typeof(ConveyorTileHolder), typeof(BaseConveyorTileHolder));
            }
            ));
        }

        private UniTask SpawnConveyor(CancellationToken token)
        {
            var count = _view.TileCountTotal;
            var initiAlizationTasks = new UniTask[count];

            for (var i = 0; i < count; i++)
            {
                var tileHolder = ResolveFromChildContainer<BaseConveyorTileHolder>();

                initiAlizationTasks[i] = tileHolder.Initialize(token);

                _tileHolders.Enqueue(tileHolder);
            }

            return UniTask.WhenAll(initiAlizationTasks);
        }



        //_______________________________

        //private readonly ConveyorProvider _conveyorProvider;
        //private readonly ITileGameObjectData _tileGameObjectData;
        //private readonly IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> _tileTileControllerFactory;
        //private readonly ConveyorPositionService _conveyorPositionService;

        //private ConveyorView _view;
        //private ConveyorTileController[] _conveyorTiles;

        //public ConveyorController(
        //    ConveyorProvider conveyorProvider,
        //    ITileGameObjectData tileGameObjectData,
        //    IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> tileTileControllerFactory,
        //    ConveyorPositionService conveyorPositionService)
        //{
        //    _conveyorProvider = conveyorProvider;
        //    _tileGameObjectData = tileGameObjectData;
        //    _tileTileControllerFactory = tileTileControllerFactory;
        //    _conveyorPositionService = conveyorPositionService;
        //}

        //public override UniTask Initialzie(CancellationToken token)
        //{
        //    return Load(token);
        //}

        //public override void Dispose()
        //{
        //    foreach(var tile in _conveyorTiles)
        //    {
        //        tile.Dispose();
        //    }

        //    base.Dispose();
        //}

        //private async UniTask Load(CancellationToken token)
        //{
        //    await UniTask.WhenAll(PreloadTilePrefab(), LoadConveyorPrefab());

        //    _conveyorPositionService.SetupConveyorData(_view.TopStart.position, _view.BottomStart.position, _view.TileCountTotal, _view.TopTileSize);

        //    await SpawnConveyor(token);
        //}

        //private async UniTask PreloadTilePrefab()
        //{
        //    var assetLoader = new AssetLoader();

        //    var gameObject = await assetLoader.Load(ConveyorConstants.ConveyorTilePrefabName);

        //    var artLength = gameObject.GetComponent<ConveyorTileView>().SpriteLength;

        //    _tileGameObjectData.SetIileGameObject(gameObject, artLength);

        //    AttachResource(assetLoader);
        //}

        //private async UniTask LoadConveyorPrefab()
        //{
        //    AttachResource(_conveyorProvider);

        //    _view = await _conveyorProvider.Instantiate();

        //    _tileGameObjectData.TilesParentTransform = _view.TilesTransform;
        //}

        //private UniTask SpawnConveyor(CancellationToken token)
        //{
        //    var count = _view.TileCountTotal;
        //    _conveyorTiles = new ConveyorTileController[count];
        //    var initiAlizationTasks = new UniTask[count];

        //    for (var i = 0; i < count; i++)
        //    {
        //        var tile = _tileTileControllerFactory.Create(new ConveyorTileControllerFactoryData(i));

        //        _conveyorTiles[i] = tile;

        //        initiAlizationTasks[i] = tile.Initialzie(token);
        //    }

        //    return UniTask.WhenAll(initiAlizationTasks);
        //}
    }
}