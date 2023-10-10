using Assets.Features.GameData.Scripts.Providers;
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
            RegisterGameData(descriptor);

            descriptor.AddTransient(typeof(RootAppController), typeof(IStartable));
            descriptor.AddSingleton(typeof(AppControllerData));

            descriptor.RegisterController<AppController>();
            descriptor.RegisterController<LoadingScreenController>();

            descriptor.AddTransient(typeof(LoadingScreenProvider));

            descriptor.AddSingleton(typeof(LoadingScreenEvents), typeof(ILoadingScreenExternalEvents), typeof(ILoadingScreenEvents));
        }

        private void RegisterGameData(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(LevelDishesTypeProvider), typeof(ILevelDishesTypeProvider));
            descriptor.AddSingleton(typeof(LevelIngridientTypeProvider), typeof(ILevelIngridientTypeProvider));
        }
    }
}