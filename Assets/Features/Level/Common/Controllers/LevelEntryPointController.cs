using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.App.LoadingScreen;
using Sushi.Level.Common.Events;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Cooking;
using Sushi.Level.Cooking.Events;
using Sushi.Level.Menu;
using Sushi.Level.WorkplaceIcon;
using Sushi.Level.WorkplaceIcon.Events;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class LevelEntryPointController : Controller
    {
        private readonly IFactory<KitchenBoardController> _kitchenBoardControllerFactory;
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;
        private readonly IFactory<LevelMenuController> _levelMenuControllerFactory;
        private readonly IFactory<CookingController> _cookingControllerFactory;

        private int _loadingFeatureCount = 4;

        public LevelEntryPointController(
            IFactory<KitchenBoardController> kitchenBoardControllerFactory,
            IFactory<ConveyorController> conveyorControllerFactory,
            IFactory<LevelMenuController> levelMenuControllerFactory,
            IFactory<CookingController> cookingControllerFactory)
        {
            _kitchenBoardControllerFactory = kitchenBoardControllerFactory;
            _conveyorControllerFactory = conveyorControllerFactory;
            _levelMenuControllerFactory = levelMenuControllerFactory;
            _cookingControllerFactory = cookingControllerFactory;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                RunChildFromFactory(_conveyorControllerFactory, linkedCts.Token).Forget();
                RunChildFromFactory(_kitchenBoardControllerFactory, linkedCts.Token).Forget();
                RunChildFromFactory(_cookingControllerFactory, linkedCts.Token).Forget();

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
            else if (controllerEvent is KitchenBoardClickEvent)
            {
                ShowCookingWindow();
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

        private void ShowCookingWindow()
        {
            InvokeDivingEvent(new ShowCookingEvent());
        }
    }
}