using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenController : ResourcefulController
    {
        private const int InittialRequestCount = 0;

        private readonly LoadingScreenProvider _provider;
        private readonly ILoadingScreenViewProvider _viewProvider;
        private readonly ILoadingScreenEvents _events;

        private LoadingScreenView _view;

        private int _loadingScreenRequestsCount;

        public LoadingScreenController(ILoadingScreenViewProvider viewProvider, LoadingScreenProvider provider, ILoadingScreenEvents events)
        {
            _viewProvider = viewProvider;
            _provider = provider;
            _events = events;

            _loadingScreenRequestsCount = InittialRequestCount;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            _events.ShowRequested += OnShowRequested;

            await LoadPrefab();

            Toggle();
        }

        public override void Dispose()
        {
            _events.ShowRequested -= OnShowRequested;

            base.Dispose();
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_provider);

            _view = await _provider.Instantiate(_viewProvider.ParrentTransfrom);

            Toggle();
        }

        private void OnShowRequested(bool isOn)
        {
            _loadingScreenRequestsCount = isOn ? ++_loadingScreenRequestsCount : --_loadingScreenRequestsCount;

            Toggle();
        }

        private void Toggle()
        {
            var shouldShow = _loadingScreenRequestsCount > 0;
            _view.Toggle(shouldShow);
        }
    }
}