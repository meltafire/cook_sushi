using Cysharp.Threading.Tasks;
using Sushi.Menu.Views;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class MenuViewController : Controller
    {
        private readonly MenuViewProvider _menuViewProvider;
        private readonly UniTaskCompletionSource _menuCompletionSource;
        private readonly ILoadingScreenExternalEvents _loadingScreenExternalEvents;

        private MenuView _view;

        public MenuViewController(
            MenuViewProvider menuViewProvider,
            ILoadingScreenExternalEvents loadingScreenExternalEvents)
        {
            _menuViewProvider = menuViewProvider;
            _loadingScreenExternalEvents = loadingScreenExternalEvents;

            _menuCompletionSource = new UniTaskCompletionSource();
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await SpawnMenu();

            RequestLoadingScreenOff();

            if (token.IsCancellationRequested)
            {
                return;
            }

            await HandleInput();
        }

        private async UniTask SpawnMenu()
        {
            AttachResource(_menuViewProvider);

            _view = await _menuViewProvider.Load();
        }

        private async UniTask HandleInput()
        {
            _view.OnButtonPressed += OnButtonPressedHappened;

            await _menuCompletionSource.Task;

            _view.OnButtonPressed -= OnButtonPressedHappened;

            RequestLoadingScreen();
        }

        private void OnButtonPressedHappened()
        {
            _menuCompletionSource.TrySetResult();
        }

        private void RequestLoadingScreen()
        {
            _loadingScreenExternalEvents.Show(true);
        }

        private void RequestLoadingScreenOff()
        {
            _loadingScreenExternalEvents.Show(false);
        }
    }
}