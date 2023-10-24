using Assets.Features.App.LoadingScreen.Scripts.Views;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenController : IController
    {
        private readonly ILoadingScreenEvents _events;
        private readonly ILoadingScreenView _view;

        public LoadingScreenController(ILoadingScreenView view, ILoadingScreenEvents events)
        {
            _view = view;
            _events = events;
        }

        public UniTask Initialize(CancellationToken token)
        {
            OnShowRequested(true);

            _events.ShowRequested += OnShowRequested;

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _events.ShowRequested -= OnShowRequested;
        }

        private void OnShowRequested(bool isOn)
        {
            _view.Toggle(isOn);
        }
    }
}