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
    public class ConveyorController : Controller
    {
        private readonly ConveyorProvider _conveyorProvider;
        private readonly ITileGameObjectData _tileGameObjectData;
        private readonly IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> _tileTileControllerFactory;
        private readonly ConveyorPositionService _conveyorPositionService;
        private readonly ILoadingStageControllerEvents _loadingStageControllerEvents;

        private ConveyorView _view;
        private CancellationToken _token;
        private UniTaskCompletionSource _completionSource;

        public ConveyorController(
            ConveyorProvider conveyorProvider,
            ITileGameObjectData tileGameObjectData,
            IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData> tileTileControllerFactory,
            ConveyorPositionService conveyorPositionService,
            ILoadingStageControllerEvents loadingStageControllerEvents)
        {
            _conveyorProvider = conveyorProvider;
            _tileGameObjectData = tileGameObjectData;
            _tileTileControllerFactory = tileTileControllerFactory;
            _conveyorPositionService = conveyorPositionService;
            _loadingStageControllerEvents = loadingStageControllerEvents;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            _token = token;

            _loadingStageControllerEvents.LoadRequest += OnLoadRequested;

            await _completionSource.Task;
        }

        private void OnLoadRequested()
        {
            Load().Forget();
        }

        private async UniTask Load()
        {
            _loadingStageControllerEvents.ReportStartedLoading();

            await UniTask.WhenAll(PreloadTilePrefab(), LoadConveyorPrefab());

            _conveyorPositionService.SetupConveyorData(_view.TopStart.position, _view.BottomStart.position, _view.TileCountTotal, _view.TopTileSize);

            await SpawnConveyor(_token);
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

        private async UniTask SpawnConveyor(CancellationToken token)
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

            _loadingStageControllerEvents.LoadRequest -= OnLoadRequested;
            _loadingStageControllerEvents.ReportLoaded();

            await UniTask.WhenAll(tileTasks);

            HandleTilesDone();
        }

        private void HandleTilesDone()
        {
            _completionSource.TrySetResult();
        }
    }
}