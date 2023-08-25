using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Level.Conveyor.Controllers;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class RootLevelController : Controller
    {
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;

        public RootLevelController(
            IFactory<ConveyorController> conveyorControllerFactory)
        {
            _conveyorControllerFactory = conveyorControllerFactory;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            await RunChild(_conveyorControllerFactory, token);

            BubbleEvent?.Invoke(new RootAppEvent(AppActionType.Menu));
        }
    }
}