using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.App.LoadingScreen;
using Sushi.Level.Common.Events;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Menu;
using Sushi.Level.WorkplaceIcon;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class LevelEntryPointController : Controller
    {
        private readonly IFactory<KitchenBoardController> _kitchenBoardController;
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;
        private readonly IFactory<LevelMenuController> _levelMenuControllerFactory;

        private int _loadingFeatureCount = 3;

        public LevelEntryPointController(
            IFactory<KitchenBoardController> kitchenBoardController,
            IFactory<ConveyorController> conveyorControllerFactory,
            IFactory<LevelMenuController> levelMenuControllerFactory)
        {
            _kitchenBoardController = kitchenBoardController;
            _conveyorControllerFactory = conveyorControllerFactory;
            _levelMenuControllerFactory = levelMenuControllerFactory;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                RunChildFromFactory(_conveyorControllerFactory, linkedCts.Token).Forget();
                RunChildFromFactory(_kitchenBoardController, linkedCts.Token).Forget();

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

                    RequestGameplay();
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

        private void RequestGameplay()
        {
            InvokeDivingEvent(new GameplayLaunchEvent());
        }
    }
}