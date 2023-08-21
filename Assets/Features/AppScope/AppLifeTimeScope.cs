using Sushi.App.Installer;
using Sushi.Menu.Installer;
using Sushi.SceneReference;
using UnityEngine;
using Utils.AddressableLoader;
using VContainer;
using VContainer.Unity;

namespace Sushi.AppScope
{
    public class AppLifeTimeScope : LifetimeScope
    {
        [SerializeField]
        private SceneHandler _sceneHandler;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterAssetLoader(builder);
            RegisterSceneReferences(builder);
            RegisterAppFeature(builder);
            RegisterMenuFeature(builder);
        }

        private void RegisterAssetLoader(IContainerBuilder builder)
        {
            builder.Register<AssetLoader>(Lifetime.Transient);
        }

        private void RegisterSceneReferences(IContainerBuilder builder)
        {
            builder.RegisterInstance<ISceneReference>(_sceneHandler);
        }

        private void RegisterAppFeature(IContainerBuilder builder)
        {
            var appInstaller = new AppInstaller();

            appInstaller.Install(builder);
        }

        private void RegisterMenuFeature(IContainerBuilder builder)
        {
            builder.Register<MenuInstaller>(Lifetime.Transient);
        }
    }
}