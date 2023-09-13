using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public class CookingController : Controller
    {
        private readonly CookingViewProvider _cookingViewProvider;
        private readonly CookingUiProvider _cookingUiProvider;
        private readonly ILoadingStageControllerEvents _loadingStageControllerEvents;
        private readonly ICookingStageExternalEvents _cookingStageExternalEvents;

        private CookingView _view;
        private CookingUiView _uiView;
        private UniTaskCompletionSource _completionSource;

        public CookingController(
            CookingViewProvider cookingViewProvider,
            CookingUiProvider cookingUiProvider,
            ILoadingStageControllerEvents loadingStageControllerEvents,
            ICookingStageExternalEvents cookingStageExternalEvents)
        {
            _cookingViewProvider = cookingViewProvider;
            _cookingUiProvider = cookingUiProvider;
            _loadingStageControllerEvents = loadingStageControllerEvents;
            _cookingStageExternalEvents = cookingStageExternalEvents;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _loadingStageControllerEvents.LoadRequest += OnLoadRequested;
            _cookingStageExternalEvents.ShowRequest += OnShowWindowRequest;

            _completionSource = new UniTaskCompletionSource();
            token.Register(OnCancellationRequested);

            await _completionSource.Task;

            _loadingStageControllerEvents.LoadRequest -= OnLoadRequested;
            _cookingStageExternalEvents.ShowRequest -= OnShowWindowRequest;
        }

        private void OnShowWindowRequest(bool shouldShow)
        {
            ShowWindow(shouldShow);
        }

        private void OnLoadRequested()
        {
            _loadingStageControllerEvents.ReportStartedLoading();

            LoadPrefabs().Forget();
        }

        private async UniTask LoadPrefabs()
        {
            await UniTask.WhenAll(LoadPrefab(), LoadUiPrefab());

            ShowWindow(false);

            _loadingStageControllerEvents.ReportLoaded();
        }

        private void ShowWindow(bool shouldShow)
        {
            _view.Toggle(shouldShow);
            _uiView.Toggle(shouldShow);

            if (shouldShow)
            {
                _uiView.OnBackButtonClick += OnBackButtonClickHappen;
            }
            else
            {
                _uiView.OnBackButtonClick -= OnBackButtonClickHappen;
            }
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_cookingViewProvider);

            _view = await _cookingViewProvider.Load();
        }

        private async UniTask LoadUiPrefab()
        {
            AttachResource(_cookingUiProvider);

            _uiView = await _cookingUiProvider.Instantiate();
        }

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }

        private void OnBackButtonClickHappen()
        {
            _cookingStageExternalEvents.ReportBackButtonClicked();
        }
    }
}