using Reflex.Core;
using Sushi.App.Data;

namespace Sushi.App.Installer
{
    public class AppInstaller : IInstaller
    {
        public void InstallBindings(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(RootAppController), typeof(IStartable));
            descriptor.AddSingleton(typeof(AppControllerData));
        }
    }
}