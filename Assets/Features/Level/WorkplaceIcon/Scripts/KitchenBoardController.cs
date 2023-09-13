using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardController : Controller
    {
        private readonly KitchenBoardProvider _kitchenBoardProvider;
        private readonly ILoadingStageControllerEvents _loadingStageControllerEvents;
        private readonly IIdleStageControllerEvents _idleStageControllerEvents;
        private readonly IKitchenBoardIconEvents _kitchenBoardIconEvents;

        private KitchenBoardView _view;
        private UniTaskCompletionSource _completionSource;

        public KitchenBoardController(
            KitchenBoardProvider kitchenBoardProvider,
            ILoadingStageControllerEvents loadingStageControllerEvents,
            IIdleStageControllerEvents idleStageControllerEvents,
            IKitchenBoardIconEvents kitchenBoardIconEvents)
        {
            _kitchenBoardProvider = kitchenBoardProvider;
            _loadingStageControllerEvents = loadingStageControllerEvents;
            _idleStageControllerEvents = idleStageControllerEvents;
            _kitchenBoardIconEvents = kitchenBoardIconEvents;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            _loadingStageControllerEvents.LoadRequest += OnLoadRequested;
            _idleStageControllerEvents.LaunchGameplayRequest += OnLaunchGameplayRequest;

            token.Register(OnCancellationRequested);

            await _completionSource.Task;

            _loadingStageControllerEvents.LoadRequest -= OnLoadRequested;
            _idleStageControllerEvents.LaunchGameplayRequest -= OnLaunchGameplayRequest;
            _view.OnClick -= OnClickHappened;
        }

        private void OnLoadRequested()
        {
            _loadingStageControllerEvents.ReportStartedLoading();
            LoadConveyorPrefab().Forget();
        }

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }

        private async UniTask LoadConveyorPrefab()
        {
            AttachResource(_kitchenBoardProvider);

            _view = await _kitchenBoardProvider.Load();

           _loadingStageControllerEvents.ReportLoaded();
        }

        private void OnLaunchGameplayRequest()
        {
            _view.OnClick += OnClickHappened;
        }

        private void OnClickHappened()
        {
            _kitchenBoardIconEvents.ReportButtonClick();
        }
    }
}