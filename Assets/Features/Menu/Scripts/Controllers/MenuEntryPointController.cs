using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public class MenuEntryPointController : Controller
    {
        private readonly IFactory<MenuViewController> _menuViewControllerFactory;

        public MenuEntryPointController(
            IFactory<MenuViewController> menuViewControllerFactory)
        {
            _menuViewControllerFactory = menuViewControllerFactory;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await _menuViewControllerFactory.Create().RunChild(this, token);

            BubbleEvent?.Invoke(new RootAppEvent(AppActionType.Level));
        }
    }
}