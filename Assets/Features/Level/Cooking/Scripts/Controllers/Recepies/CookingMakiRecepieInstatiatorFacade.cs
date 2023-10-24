using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Recepies
{
    public class CookingMakiRecepieInstatiatorFacade : ContainerFacade
    {
        private static readonly string ContainerName = "CookingMakiRecepieInstatiatorFacade";

        private IController _controller;

        public CookingMakiRecepieInstatiatorFacade(Container container) : base(container)
        {
        }

        protected override void ActAfterContainerDisposed()
        {
            _controller.Dispose();
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _controller = ResolveFromChildContainer<UniqueVcFacade<CookingMakiRecepieController, ButtonView>>();

            return _controller.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
        }

        protected override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            return UniTask.FromResult(
               Container.Scope(ContainerName, descriptor =>
           {
               descriptor.AddTransient(typeof(CookingMakiRecepieFacade), typeof(UniqueVcFacade<CookingMakiRecepieController, ButtonView>));
               descriptor.AddTransient(typeof(CookingMakiRecepieInstantiator), typeof(AssetInstantiator<ButtonView>));
           })
               );
        }
    }
}