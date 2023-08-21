using VContainer;
using VContainer.Unity;

namespace Sushi.Menu.Installer
{
    public class MenuInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RootMenuController>();
        }
    }
}