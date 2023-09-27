using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenController : ResourcefulController
    {
        private readonly LoadingScreenProvider _provider;
        private readonly ILoadingScreenViewProvider _viewProvider;
        private readonly ILoadingScreenEvents _events;

        private LoadingScreenView _view;

        public LoadingScreenController(ILoadingScreenViewProvider viewProvider, LoadingScreenProvider provider, ILoadingScreenEvents events)
        {
            _viewProvider = viewProvider;
            _provider = provider;
            _events = events;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            _events.ShowRequested += OnShowRequested;

            await LoadPrefab();
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
        }

        private void OnShowRequested(bool isOn)
        {
            _view.Toggle(isOn);
        }
    }
}