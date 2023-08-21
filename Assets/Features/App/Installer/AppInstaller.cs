using VContainer.Unity;
using VContainer;
using Sushi.App.Data;
using Sushi.App.Events;

namespace Sushi.App.Installer
{
    public class AppInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RootAppController>();
            builder.Register<AppControllerData>(Lifetime.Transient);

            builder.Register<AppEventBus>(Lifetime.Singleton)
                .As<IAppEventProvider, IAppMenuEventInvoker>();
        }
    }
}