using Cysharp.Threading.Tasks;
using Sushi.App.Events;
using System.Threading;
using Utils.Controllers;
using VContainer.Unity;

namespace Sushi.Menu
{
    public class RootMenuController : Controller, IAsyncStartable
    {
        private readonly IAppMenuEventInvoker _appMenuEventInvoker;

        public RootMenuController(IAppMenuEventInvoker appMenuEventInvoker)
        {
            _appMenuEventInvoker = appMenuEventInvoker;
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            return Run(cancellation);
        }

        protected async override UniTask Run(CancellationToken token)
        {
            _appMenuEventInvoker.RequestFeatureWorkComletion();
        }
    }
}