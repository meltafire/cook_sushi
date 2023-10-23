using Assets.Features.Menu.Scripts.Events;
using Reflex.Core;
using Sushi.Menu.Controllers;

namespace Sushi.Menu.Installer
{
    public class MenuInstaller : IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(MenuControllerEvents), typeof(IMenuControllerEvents), typeof(IMenuControllerExternalEvents));

            descriptor.AddTransient(typeof(MenuFacade), typeof(BaseMenuFacade));

            descriptor.AddTransient(typeof(MenuViewProvider));
        }
    }
}