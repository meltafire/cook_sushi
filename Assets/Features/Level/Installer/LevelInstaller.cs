using Sushi.Level.Common.Controllers;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Services;
using Utils.Controllers.VContainerIntegration;
using VContainer;
using VContainer.Unity;

namespace Sushi.Level.Installer
{
    public class LevelInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterController<RootLevelController>();
            builder.RegisterController<ConveyorController>();

            InstallConveyor(builder);
        }

        private void InstallConveyor(IContainerBuilder builder)
        {
            builder.Register<ConveyorGameObjectData>(Lifetime.Scoped)
                .As<IConveyorGameObjectData, IConveyorPointProvider>();

            builder.Register<TileGameObjectData>(Lifetime.Scoped)
                .As<ITileGameObjectData, ITileGameObjectDataProvider, ITileGameObjectDimensionProvider>();

            builder.Register<ConveyorTileControllerFactory>(Lifetime.Transient);

            builder.Register<ConveyorTilePositionService>(Lifetime.Transient);
            builder.Register<ConveyorPositionService>(Lifetime.Transient);
        }
    }
}