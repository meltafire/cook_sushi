using Reflex.Core;
using Sushi.Menu.Controllers;

namespace Sushi.Menu.Installer
{
    public class MenuInstaller : IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(MenuFacade), typeof(BaseMenuFacade));

            descriptor.AddTransient(typeof(MenuViewProvider));
        }
    }
}