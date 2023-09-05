using Reflex.Core;
using Sushi.App.Data;
using Sushi.App.LoadingScreen;
using Utils.Controllers.ReflexIntegration;

namespace Sushi.App.Installer
{
    public class AppInstaller : IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.AddTransient(typeof(RootAppController), typeof(IStartable));
            descriptor.AddSingleton(typeof(AppControllerData));

            descriptor.RegisterController<AppController>();
            descriptor.RegisterController<LoadingScreenController>();
        }
    }
}