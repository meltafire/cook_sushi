using Reflex.Core;
using Sushi.Level.Common.Controllers;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Menu.Controllers;
using Utils.Controllers.ReflexIntegration;

namespace Sushi.Level.Installer
{
    public class LevelInstaller: IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.RegisterController<RootLevelController>();

            InstallConveyor(descriptor);
            InstallMenu(descriptor);
        }

        private void InstallConveyor(ContainerDescriptor descriptor)
        {
            descriptor.RegisterController<ConveyorController>();

            descriptor.AddSingleton(typeof(ConveyorGameObjectData),
                typeof(IConveyorGameObjectData),
                typeof(IConveyorPointProvider));

            descriptor.AddSingleton(typeof(TileGameObjectData),
                typeof(ITileGameObjectData),
                typeof(ITileGameObjectDataProvider),
                typeof(ITileGameObjectDimensionProvider));

            descriptor.AddTransient(typeof(ConveyorTileControllerFactory));

            descriptor.AddTransient(typeof(ConveyorTilePositionService));
            descriptor.AddTransient(typeof(ConveyorPositionService));
        }

        private void InstallMenu(ContainerDescriptor descriptor)
        {
            descriptor.RegisterController<LevelMenuController>();
        }
    }
}