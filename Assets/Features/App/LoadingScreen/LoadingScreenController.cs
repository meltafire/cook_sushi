using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenController : Controller
    {
        private const int InittialRequestCount = 1;

        private readonly ILoadingScreenViewProvider _viewProvider;
        private readonly ILoadingScreenEvents _events;

        private LoadingScreenView _view;
        private UniTaskCompletionSource _completionSource;

        private int _loadingScreenRequestsCount;

        public LoadingScreenController(ILoadingScreenViewProvider viewProvider, ILoadingScreenEvents events)
        {
            _viewProvider = viewProvider;
            _events = events;

            _loadingScreenRequestsCount = InittialRequestCount;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            _view = _viewProvider.View;

            _events.ShowRequested += OnShowRequested;

            await _completionSource.Task;

            _events.ShowRequested -= OnShowRequested;
        }

        private void OnShowRequested(bool isOn)
        {
            _loadingScreenRequestsCount = isOn ? ++_loadingScreenRequestsCount : --_loadingScreenRequestsCount;

            var shouldShow = _loadingScreenRequestsCount > 0;
            _view.Toggle(shouldShow);
        }
    }
}