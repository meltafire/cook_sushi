using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Cooking;
using Sushi.Level.Menu;
using Sushi.Level.WorkplaceIcon;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class LevelEntryPointController : Controller
    {
        private readonly ILoadingScreenExternalEvents _loadingScreenExternalEvents;
        private readonly Dictionary<LevelStages, IStage> _stages;


        private readonly IFactory<KitchenBoardController> _kitchenBoardControllerFactory;
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;
        private readonly IFactory<LevelMenuController> _levelMenuControllerFactory;
        private readonly IFactory<CookingController> _cookingControllerFactory;

        public LevelEntryPointController(
            ILoadingScreenExternalEvents loadingScreenExternalEvents,
            LoadingStage loadingStage,
            IdleStage idleStage,
            CookingStage cookingStage,


            IFactory<KitchenBoardController> kitchenBoardControllerFactory,
            IFactory<ConveyorController> conveyorControllerFactory,
            IFactory<LevelMenuController> levelMenuControllerFactory,
            IFactory<CookingController> cookingControllerFactory)
        {
            _loadingScreenExternalEvents = loadingScreenExternalEvents;

            _stages = new Dictionary<LevelStages, IStage>()
            {
                {LevelStages.Loading, loadingStage},
                {LevelStages.Idle, idleStage},
                {LevelStages.Cooking, cookingStage},
            };

            _kitchenBoardControllerFactory = kitchenBoardControllerFactory;
            _conveyorControllerFactory = conveyorControllerFactory;
            _levelMenuControllerFactory = levelMenuControllerFactory;
            _cookingControllerFactory = cookingControllerFactory;
        }

        protected async override UniTask Run(CancellationToken token)
        {
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                var linkedToken = linkedCts.Token;

                RunChildFromFactory(_conveyorControllerFactory, linkedToken).Forget();
                RunChildFromFactory(_kitchenBoardControllerFactory, linkedToken).Forget();
                RunChildFromFactory(_cookingControllerFactory, linkedToken).Forget();
                RunChildFromFactory(_levelMenuControllerFactory, linkedToken).Forget();

                await RunStages(linkedToken);

                linkedCts.Cancel();
            }

            RequestLoadingScreen();

            InvokeBubbleEvent(new RootAppEvent(AppActionType.Menu));
        }

        private async UniTask RunStages(CancellationToken token)
        {
            var stageType = LevelStages.Loading;

            while (stageType != LevelStages.Quit)
            {
                stageType = await _stages[stageType].Run(token);
            }
        }

        private void RequestLoadingScreen()
        {
            _loadingScreenExternalEvents.Show(true);
        }
    }
}