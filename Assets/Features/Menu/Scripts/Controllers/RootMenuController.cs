using Cysharp.Threading.Tasks;
using Sushi.App.Events;
using System.Threading;
using Utils.Controllers;
using VContainer.Unity;

namespace Sushi.Menu.Controllers
{
    public class RootMenuController : Controller, IAsyncStartable
    {
        private readonly IAppMenuEventInvoker _appMenuEventInvoker;
        private readonly IFactory<MenuViewController> _menuViewControllerFactory;

        public RootMenuController(
            IAppMenuEventInvoker appMenuEventInvoker,
            IFactory<MenuViewController> menuViewControllerFactory)
        {
            _appMenuEventInvoker = appMenuEventInvoker;
            _menuViewControllerFactory = menuViewControllerFactory;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            return Run(cancellation);
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await RunChild(_menuViewControllerFactory, token);


            _appMenuEventInvoker.RequestFeatureWorkCompletion();
        }
    }
}