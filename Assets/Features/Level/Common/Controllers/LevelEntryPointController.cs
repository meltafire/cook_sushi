using Assets.Features.Level.Conveyor.Scripts;
using Assets.Features.Level.Cooking.Scripts.Controllers;
using Assets.Features.Level.Menu.Scripts;
using Assets.Features.Level.WorkplaceIcon.Scripts;
using Cysharp.Threading.Tasks;
using Sushi.Level.Menu;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Common.Controllers
{
    public interface ILevelEntryPointController : IController
    {
        UniTask Launch(CancellationToken token);
    }

    public class LevelEntryPointController : ILevelEntryPointController
    {
        private readonly BaseKitchenBoardFacade _kitchenBoardFacade;
        private readonly BaseConveyorFacade _baseConveyorFacade;
        private readonly BaseLevelMenuFacade _baseLevelMenuFacade;
        private readonly IFactory<RootCookingController> _rootCookingControllerFactory;
        private readonly LevelStagesFacade _levelStagesFacade;

        private RootCookingController _rootCookingController;

        public LevelEntryPointController(
            BaseKitchenBoardFacade kitchenBoardFacade,
            BaseConveyorFacade baseConveyorFacade,
            BaseLevelMenuFacade baseLevelMenuFacade,
            IFactory<RootCookingController> rootCookingControllerFactory,
            LevelStagesFacade levelStagesFacade)
        {
            _kitchenBoardFacade = kitchenBoardFacade;
            _baseConveyorFacade = baseConveyorFacade;
            _baseLevelMenuFacade = baseLevelMenuFacade;
            _rootCookingControllerFactory = rootCookingControllerFactory;
            _levelStagesFacade = levelStagesFacade;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            await _kitchenBoardFacade.Initialize(token);

            await _baseConveyorFacade.Initialize(token);

            await _baseLevelMenuFacade.Initialize(token);

            _rootCookingController = _rootCookingControllerFactory.Create();
            await _rootCookingController.Initialize(token);
        }

        public void Dispose()
        {
            _kitchenBoardFacade.Dispose();
            _baseConveyorFacade.Dispose();
            _baseLevelMenuFacade.Dispose();
            _rootCookingController.Dispose();
        }

        public UniTask Launch(CancellationToken token)
        {
            return _levelStagesFacade.Launch(token);
        }
    }
}