using Assets.Features.App.LoadingScreen.Scripts.Views;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.App.LoadingScreen;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.App.LoadingScreen.Scripts
{
    public class LoadingScreenFacade : ContainerFacade
    {
        private static string ContainerName = "MenuFacade";

        private readonly LoadingScreenProvider _provider;

        private LoadingScreenController _loadingScreenController;

        public LoadingScreenFacade(
            Container container,
            LoadingScreenProvider provider)
            : base(container)
        {
            _provider = provider;
        }

        protected override async UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _provider.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(ILoadingScreenView));
                descriptor.AddTransient(typeof(LoadingScreenController));
            });
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _loadingScreenController = ResolveFromChildContainer<LoadingScreenController>();

            return _loadingScreenController.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _loadingScreenController.Dispose();
        }

        protected override void ActAfterContainerDisposed()
        {
            _provider.Unload();
        }
    }
}