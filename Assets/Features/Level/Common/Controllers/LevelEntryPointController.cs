using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.App.LoadingScreen;
using Sushi.Level.Common.Events;
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

        private int _loadingFeatureCount = 2;

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

            RequestLoadingScreen();

            InvokeBubbleEvent(new RootAppEvent(AppActionType.Menu));
        }

        protected override void HandleBubbleEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is LevelFeatureLoadedEvent)
            {
                --_loadingFeatureCount;

                if (_loadingFeatureCount <= 0)
                {
                    RequestLoadingScreenOff();
                }
            }
        }

        private void RequestLoadingScreenOff()
        {
            InvokeBubbleEvent(new LoadingScreenEvent(false));
        }

        private void RequestLoadingScreen()
        {
            InvokeBubbleEvent(new LoadingScreenEvent(true));
        }
    }
}