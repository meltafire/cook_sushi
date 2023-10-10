using Reflex.Core;
using Sushi.Level.Common.Controllers;
using Sushi.Level.Conveyor;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Factory;
using Sushi.Level.Conveyor.Factory.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Cooking;
using Sushi.Level.Menu;
using Sushi.Level.WorkplaceIcon;
using Utils.Controllers;
using Utils.Controllers.ReflexIntegration;
using Assets.Features.Level.Stages.Tools;
using Assets.Features.Level.Conveyor.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.Controllers;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;

namespace Sushi.Level.Installer
{
    public class LevelInstaller : IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.RegisterController<LevelEntryPointController>();

            InstallConveyor(descriptor);
            InstallKitchenBoard(descriptor);
            InstallMenu(descriptor);
            InstallCooking(descriptor);
            InstallStages(descriptor);
        }

        private void InstallConveyor(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(ConveyorProvider));

            descriptor.RegisterController<ConveyorController>();

            descriptor.AddSingleton(typeof(ConveyorGameObjectData),
                typeof(IConveyorGameObjectData),
                typeof(IConveyorPointProvider));

            descriptor.AddSingleton(typeof(TileGameObjectData),
                typeof(ITileGameObjectData),
                typeof(ITileGameObjectDataProvider),
                typeof(ITileGameObjectDimensionProvider));

            descriptor.AddTransient(typeof(ConveyorTileControllerFactory), typeof(IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData>));

            descriptor.AddTransient(typeof(ConveyorTilePositionService));
            descriptor.AddTransient(typeof(ConveyorPositionService));

            descriptor.AddSingleton(typeof(ConveyorTileEvents),
                typeof(IConveyorTileExternalEvents),
                typeof(IConveyorTileEvents));
        }

        private void InstallMenu(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(LevelMenuProvider));

            descriptor.RegisterController<LevelMenuController>();

            descriptor.AddSingleton(typeof(LevelMenuEvents), typeof(ILevelMenuEvents), typeof(ILevelMenuExternalEvents));
        }

        private void InstallKitchenBoard(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(KitchenBoardProvider));

            descriptor.RegisterController<KitchenBoardController>();

            descriptor.AddSingleton(typeof(KitchenBoardIconEvents), typeof(IKitchenBoardIconEvents), typeof(IKitchenBoardIconExternalEvents));
        }

        private void InstallCooking(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(CookingViewProvider));
            descriptor.AddTransient(typeof(CookingUiProvider));

            descriptor.RegisterController<RootCookingController>();

            descriptor.AddSingleton(typeof(CookingControllerEvents),
                        typeof(ICookingControllerEvents),
                        typeof(ICookingControllerExternalEvents)
                        );
        }

        private void InstallStages(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(LevelStagesFacade));
            descriptor.AddTransient(typeof(IdleStage));
            descriptor.AddTransient(typeof(CookingStage));

            descriptor.RegisterStage<IdleStage>();
            descriptor.RegisterStage<CookingStage>();
        }
    }
}