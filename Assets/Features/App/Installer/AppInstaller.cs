using VContainer.Unity;
using VContainer;
using Sushi.App.Data;

namespace Sushi.App.Installer
{
    public class AppInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RootAppController>();
            builder.Register<AppControllerData>(Lifetime.Transient);
        }
    }
}