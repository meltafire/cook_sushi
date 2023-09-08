using Reflex.Core;
using Sushi.Menu.Controllers;
using Utils.Controllers.ReflexIntegration;

namespace Sushi.Menu.Installer
{
    public class MenuInstaller : IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.RegisterController<MenuEntryPointController>();
            descriptor.RegisterController<MenuViewController>();

            descriptor.AddTransient(typeof(MenuViewProvider));
        }
    }
}