using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Menu.Controllers;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class LevelEntryPointController : Controller
    {
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;
        private readonly IFactory<LevelMenuController> _levelMenuControllerFactory;

        public LevelEntryPointController(
            IFactory<ConveyorController> conveyorControllerFactory,
            IFactory<LevelMenuController> levelMenuControllerFactory)
        {
            _conveyorControllerFactory = conveyorControllerFactory;
            _levelMenuControllerFactory = levelMenuControllerFactory;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                RunChildFromFactory(_conveyorControllerFactory, linkedCts.Token).Forget();

                await RunChildFromFactory(_levelMenuControllerFactory, linkedCts.Token);

                linkedCts.Cancel();
            }

            InvokeBubbleEvent(new RootAppEvent(AppActionType.Menu));
        }
    }
}