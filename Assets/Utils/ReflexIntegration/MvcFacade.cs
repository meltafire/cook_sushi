using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public abstract class MvcFacade<T, U> : ContainerFacade where T : IController
    {
        protected abstract string ContainerName { get; }

        private readonly AssetInstantiator<U> _assetInstantiator;

        private IController _controller;

        protected MvcFacade(
            AssetInstantiator<U> assetInstantiator,
            Container container) 
            : base(container)
        {
            _assetInstantiator = assetInstantiator;
        }

        protected override void ActAfterContainerDisposed()
        {
            _assetInstantiator.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _controller = ResolveFromChildContainer<IController>();

            return _controller.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _controller.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _assetInstantiator.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(U));
                descriptor.AddTransient(typeof(T), typeof(IController));
            });
        }
    }
}