using Cysharp.Threading.Tasks;
using System.Threading;

namespace Utils.Controllers
{
    public abstract class Controller
    {
        protected async UniTask RunChild<T>(IFactory<T> controllerFactory, CancellationToken token) where T : Controller
        {
            var controller = controllerFactory.Create();

            await controller.Run(token);
        }

        protected abstract UniTask Run(CancellationToken token);
    }
}