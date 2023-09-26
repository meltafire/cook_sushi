using Assets.Features.Menu.Scripts.Data;
using Cysharp.Threading.Tasks;
using Sushi.Menu.Views;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class MenuViewController : BaseMenuViewController
    {
        private readonly MenuViewProvider _menuViewProvider;

        private UniTaskCompletionSource _menuCompletionSource;
        private MenuView _view;

        public MenuViewController(MenuViewProvider menuViewProvider)
        {
            _menuViewProvider = menuViewProvider;
        }

        public override UniTask Initialzie(CancellationToken token)
        {
            return SpawnMenu();
        }

        public override async UniTask<MenuResults> Launch(CancellationToken token)
        {
            await HandleInput();

            return MenuResults.Level;
        }

        private async UniTask HandleInput()
        {
            _menuCompletionSource = new UniTaskCompletionSource();

            _view.OnButtonPressed += OnButtonPressedHappened;

            await _menuCompletionSource.Task;

            _view.OnButtonPressed -= OnButtonPressedHappened;
        }

        private async UniTask SpawnMenu()
        {
            AttachResource(_menuViewProvider);

            _view = await _menuViewProvider.Load();
        }

        private void OnButtonPressedHappened()
        {
            _menuCompletionSource.TrySetResult();
        }
    }

    public abstract class BaseMenuViewController : ResourcefulController
    {
        public abstract UniTask<MenuResults> Launch(CancellationToken token);
    }
}