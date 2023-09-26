using Assets.Features.Menu.Scripts.Data;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class MenuEntryPointController : IMenuEntryPointController
    {
        private readonly IFactory<MenuViewController> _menuViewControllerFactory;

        private MenuViewController _viewController;

        public MenuEntryPointController(
            IFactory<MenuViewController> menuViewControllerFactory)
        {
            _menuViewControllerFactory = menuViewControllerFactory;
        }

        public void Dispose()
        {
            _viewController.Dispose();
        }

        public UniTask Initialzie(CancellationToken token)
        {
            _viewController = _menuViewControllerFactory.Create();

            return _viewController.Initialzie(token);
        }

        public UniTask<MenuResults> Launch(CancellationToken token)
        {
            return _viewController.Launch(token);
        }
    }

    public interface IMenuEntryPointController : IController
    {
        public UniTask<MenuResults> Launch(CancellationToken token);
    }
}