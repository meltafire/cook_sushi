using Reflex.Core;
using Sushi.App.Installer;
using Sushi.App.LoadingScreen;
using Sushi.Level.Installer;
using Sushi.Menu.Installer;
using Sushi.SceneReference;
using System.Threading;
using UnityEngine;

namespace Sushi.AppScope
{
    public class AppLifeTimeScope: MonoBehaviour, IInstaller
    {
        [SerializeField]
        private SceneHandler _sceneHandler;

        [SerializeField]
        private LoadingScreenParentTransformProvider _loadingScreenTransform;

        private CancellationTokenSource _cancellationTokenSource;

        public void OnDestroy()
        {
            //_cancellationTokenSource.Cancel();
        }

        public void InstallBindings(ContainerDescriptor descriptor)
        {
            RegisterSceneReferences(descriptor);
            RegisterCommonCancellationToken(descriptor);
            RegisterAppFeature(descriptor);
            RegisterMenuFeature(descriptor);
            RegisterLevelFeature(descriptor);
        }

        private void RegisterSceneReferences(ContainerDescriptor descriptor)
        {
            descriptor.AddInstance(_sceneHandler, typeof(ISceneReference), typeof(IStageRootParentTransformProvider));
        }

        private void RegisterCommonCancellationToken(ContainerDescriptor descriptor)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            descriptor.AddInstance(_cancellationTokenSource.Token, typeof(CancellationToken));
        }

        private void RegisterAppFeature(ContainerDescriptor descriptor)
        {
            var appInstaller = new AppInstaller();

            descriptor.AddInstance(_loadingScreenTransform, typeof(ILoadingScreenParentTransformProvider));

            appInstaller.InstallBindings(descriptor);
        }

        private void RegisterMenuFeature(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(MenuInstaller));
        }

        private void RegisterLevelFeature(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(LevelInstaller));
        }
    }
}