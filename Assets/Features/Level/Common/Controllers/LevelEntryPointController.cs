using Assets.Features.Level.Cooking.Scripts.Controllers;
using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Cooking;
using Sushi.Level.Menu;
using Sushi.Level.WorkplaceIcon;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public class LevelEntryPointController : ILaunchableController
    {
        private readonly IFactory<KitchenBoardController> _kitchenBoardControllerFactory;
        private readonly IFactory<ConveyorController> _conveyorControllerFactory;
        private readonly IFactory<LevelMenuController> _levelMenuControllerFactory;
        private readonly IFactory<RootCookingController> _rootCookingControllerFactory;
        private readonly LevelStagesFacade _levelStagesFacade;

        private KitchenBoardController _kitchenBoardController;
        private ConveyorController _conveyorController;
        private LevelMenuController _levelMenuController;
        private RootCookingController _rootCookingController;

        public LevelEntryPointController(
            IFactory<KitchenBoardController> kitchenBoardControllerFactory,
            IFactory<ConveyorController> conveyorControllerFactory,
            IFactory<LevelMenuController> levelMenuControllerFactory,
            IFactory<RootCookingController> rootCookingControllerFactory,
            LevelStagesFacade levelStagesFacade)
        {
            _kitchenBoardControllerFactory = kitchenBoardControllerFactory;
            _conveyorControllerFactory = conveyorControllerFactory;
            _levelMenuControllerFactory = levelMenuControllerFactory;
            _rootCookingControllerFactory = rootCookingControllerFactory;
            _levelStagesFacade = levelStagesFacade;
        }

        public async UniTask Initialzie(CancellationToken token)
        {
            _kitchenBoardController = _kitchenBoardControllerFactory.Create();
            await _kitchenBoardController.Initialzie(token);

            _conveyorController = _conveyorControllerFactory.Create();
            await _conveyorController.Initialzie(token);

            _levelMenuController = _levelMenuControllerFactory.Create();
            await _levelMenuController.Initialzie(token);

            _rootCookingController = _rootCookingControllerFactory.Create();
            await _rootCookingController.Initialzie(token);
        }

        public void Dispose()
        {
            _kitchenBoardController.Dispose();
            _conveyorController.Dispose();
            _levelMenuController.Dispose();
            _rootCookingController.Dispose();
        }

        public UniTask Launch(CancellationToken token)
        {
            return  _levelStagesFacade.Launch(token);
        }
    }
}