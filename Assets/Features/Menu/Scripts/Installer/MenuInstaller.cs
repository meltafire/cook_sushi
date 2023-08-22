using Sushi.Menu.Controllers;
using Utils.Controllers.VContainerIntegration;
using VContainer;
using VContainer.Unity;

namespace Sushi.Menu.Installer
{
    public class MenuInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterController<RootMenuController>();
            builder.RegisterController<MenuViewController>();
        }
    }
}