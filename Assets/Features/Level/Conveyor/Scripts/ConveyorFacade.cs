using Assets.Features.Level.Conveyor.Scripts.Views;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor;
using Sushi.Level.Conveyor.Controllers;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Conveyor.Scripts
{
    public class ConveyorFacade : BaseConveyorFacade
    {
        private static readonly string ContainerName = "ConveyorFacade";

        private readonly ConveyorInstantiator _conveyorProvider;

        private IController _coveyorController;

        public ConveyorFacade(Container container, ConveyorInstantiator conveyorProvider) : base(container)
        {
            _conveyorProvider = conveyorProvider;
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _coveyorController = ResolveFromChildContainer<BaseConveyorController>();

            return _coveyorController.Initialize(token);
        }

        protected override void ActAfterContainerDisposed()
        {
            _conveyorProvider.Unload();
        }

        protected override void ActBeforeContainerDisposed()
        {
            _coveyorController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _conveyorProvider.Load();

            view.gameObject.SetActive(true);

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(IConveyorView));
                descriptor.AddTransient(typeof(ConveyorController), typeof(BaseConveyorController));
            });
        }
    }
}