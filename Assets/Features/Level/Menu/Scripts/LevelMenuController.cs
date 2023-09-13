using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Menu
{
    public class LevelMenuController : Controller
    {
        private readonly LevelMenuProvider _levelMenuProvider;
        private readonly ILoadingStageControllerEvents _loadingStageControllerEvents;
        private readonly ILevelMenuEvents _levelMenuEvents;

        private LevelMenuView _view;
        private UniTaskCompletionSource _completionSource;

        public LevelMenuController(LevelMenuProvider levelMenuProvider, ILoadingStageControllerEvents loadingStageControllerEvents, ILevelMenuEvents levelMenuEvents)
        {
            _levelMenuProvider = levelMenuProvider;
            _loadingStageControllerEvents = loadingStageControllerEvents;
            _levelMenuEvents = levelMenuEvents;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();

            _loadingStageControllerEvents.LoadRequest += OnLoadRequested;

            token.Register(OnCanceltaionRequested);

            await _completionSource.Task;

            UnsubscribeFromView();

            _loadingStageControllerEvents.LoadRequest -= OnLoadRequested;
        }

        private void OnLoadRequested()
        {
            _loadingStageControllerEvents.ReportStartedLoading();

            LoadLevelMenuPrefab().Forget();
        }

        private async UniTask LoadLevelMenuPrefab()
        {
            AttachResource(_levelMenuProvider);

            _view = await _levelMenuProvider.Load();

            _loadingStageControllerEvents.ReportLoaded();

            SubscribeToView();
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
            _levelMenuEvents.HandleButtonClicked();

            _completionSource.TrySetResult();
        }

        private void OnCanceltaionRequested()
        {
            _completionSource.TrySetResult();
        }
    }
}
