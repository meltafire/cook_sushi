using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Cooking;
using Sushi.Level.Menu;
using Sushi.Level.WorkplaceIcon;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class LevelEntryPointController : ILaunchableController
    {
        private readonly ILoadingScreenExternalEvents _loadingScreenExternalEvents;
        private readonly Dictionary<LevelStages, IStage> _stages;


        private readonly IFactory<KitchenBoardController> _kitchenBoardControllerFactory;
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;
        private readonly IFactory<LevelMenuController> _levelMenuControllerFactory;
        private readonly IFactory<CookingController> _cookingControllerFactory;

        private KitchenBoardController _kitchenBoardController;
        private ConveyorController _conveyorController;
        private LevelMenuController _levelMenuController;
        private CookingController _cookingController;

        public LevelEntryPointController(
            ILoadingScreenExternalEvents loadingScreenExternalEvents,
            LoadingStage loadingStage,
            IdleStage idleStage,
            CookingStage сookingStage,


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
                {LevelStages.Cooking, сookingStage},
            };

            _kitchenBoardControllerFactory = kitchenBoardControllerFactory;
            _conveyorControllerFactory = conveyorControllerFactory;
            _levelMenuControllerFactory = levelMenuControllerFactory;
            _cookingControllerFactory = cookingControllerFactory;
        }

        public async UniTask Initialzie(CancellationToken token)
        {
            _kitchenBoardController = _kitchenBoardControllerFactory.Create();
            await _kitchenBoardController.Initialzie(token);

            _conveyorController = _conveyorControllerFactory.Create();
            await _conveyorController.Initialzie(token);

            _levelMenuController = _levelMenuControllerFactory.Create();
            await _levelMenuController.Initialzie(token);

            _cookingController = _cookingControllerFactory.Create();
            await _cookingController.Initialzie(token);
        }

        public void Dispose()
        {
            _kitchenBoardController.Dispose();
            _conveyorController.Dispose();
            _levelMenuController.Dispose();
            _cookingController.Dispose();
        }

        public async UniTask Launch(CancellationToken token)
        {
            await RunStages(token);

            RequestLoadingScreen();
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