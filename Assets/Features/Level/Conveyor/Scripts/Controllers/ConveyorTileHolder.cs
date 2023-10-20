using Assets.Features.Level.Conveyor.Scripts.Providers;
using Assets.Features.Level.Conveyor.Scripts.Views;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor.Controllers;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Conveyor.Scripts.Controllers
{
    public abstract class BaseConveyorTileHolder : ContainerFacade
    {
        protected BaseConveyorTileHolder(Container container) : base(container)
        {
        }
    }

    public class ConveyorTileHolder : BaseConveyorTileHolder
    {
        private static readonly string ContainerName = "ConveyorTileHolder";

        private readonly ConveyorTileProvider _tileProvider;

        private IController _conveyorTileController;

        public ConveyorTileHolder(Container container, ConveyorTileProvider tileProvider) : base(container)
        {
            _tileProvider = tileProvider;
        }

        protected override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _conveyorTileController = ResolveFromChildContainer<BaseConveyorTileController>();

            return _conveyorTileController.Initialize(token);
        }

        protected override void ActAfterContainerDisposed()
        {
            _tileProvider.Unload();
        }

        protected override void ActBeforeContainerDisposed()
        {
            _conveyorTileController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _tileProvider.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(IConveyorTileView));
                descriptor.AddTransient(typeof(ConveyorTileController), typeof(BaseConveyorTileController));
            });
        }
    }
}