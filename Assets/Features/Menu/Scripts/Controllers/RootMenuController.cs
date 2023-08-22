using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class RootMenuController : Controller
    {
        private readonly IFactory<MenuViewController> _menuViewControllerFactory;

        public RootMenuController(
            IFactory<MenuViewController> menuViewControllerFactory)
        {
            _menuViewControllerFactory = menuViewControllerFactory;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            return Run(cancellation);
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await RunChild(_menuViewControllerFactory, token);

            BubbleEvent?.Invoke(new RootAppEvent(AppActionType.Level));
        }
    }
}