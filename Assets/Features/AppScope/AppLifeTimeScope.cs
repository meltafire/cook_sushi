using Sushi.App;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Menu.Installer;
using VContainer;
using VContainer.Unity;

namespace Sushi.AppScope
{
    public class AppLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RootAppController>();
            builder.Register<AppControllerData>(Lifetime.Transient);

            RegisterEventBus(builder);

            RegisterFeatures(builder);
        }

        private void RegisterEventBus(IContainerBuilder builder)
        {
            builder.Register<AppEventBus>(Lifetime.Singleton)
                .As<IAppEventProvider, IAppMenuEventInvoker>();
        }

        private void RegisterFeatures(IContainerBuilder builder)
        {
            builder.Register<MenuInstaller>(Lifetime.Transient);
        }
    }
}