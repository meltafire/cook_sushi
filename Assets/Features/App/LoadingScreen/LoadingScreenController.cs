using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenController : Controller
    {
        private const int InittialRequestCount = 1;

        private readonly ILoadingScreenViewProvider _viewProvider;

        private LoadingScreenView _view;
        private UniTaskCompletionSource _completionSource;

        private int _loadingScreenRequestsCount;

        public LoadingScreenController(ILoadingScreenViewProvider viewProvider)
        {
            _viewProvider = viewProvider;
            _loadingScreenRequestsCount = InittialRequestCount;
        }

        protected override UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();

            _view = _viewProvider.View;

            return _completionSource.Task;
        }

        protected override void HandleDivingEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is LoadingScreenEvent)
            {
                var data = (LoadingScreenEvent)controllerEvent;
                _loadingScreenRequestsCount = data.ShouldShow ? ++_loadingScreenRequestsCount : --_loadingScreenRequestsCount;

                var shouldShow = _loadingScreenRequestsCount > 0;
                _view.Toggle(shouldShow);
            }
        }
    }
}