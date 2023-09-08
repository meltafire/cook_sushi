using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using Sushi.Level.Cooking.Events;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public class CookingController : Controller
    {
        private readonly CookingViewProvider _cookingViewProvider;
        private readonly CookingUiProvider _cookingUiProvider;

        private CookingView _view;
        private CookingUiView _uiView;
        private UniTaskCompletionSource _completionSource;

        public CookingController(CookingViewProvider cookingViewProvider, CookingUiProvider cookingUiProvider)
        {
            _cookingViewProvider = cookingViewProvider;
            _cookingUiProvider = cookingUiProvider;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            token.Register(OnCancellationRequested);

            await LoadPrefabs();

            ReportReady();

            await _completionSource.Task;
        }

        protected override void HandleDivingEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is ShowCookingEvent)
            {
                ShowWindow(true);
            }
        }

        private async UniTask LoadPrefabs()
        {
            await UniTask.WhenAll(LoadPrefab(), LoadUiPrefab());

            ShowWindow(false);
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

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }

        private void OnBackButtonClickHappen()
        {
            ShowWindow(false);
        }
    }
}