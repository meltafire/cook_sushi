using Assets.Features.Level.Conveyor.Scripts.Providers;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor.Views;
using System.Threading;
using Utils.AssetProvider;

namespace Assets.Features.Level.Conveyor.Scripts.Controllers
{
    public abstract class BaseConveyorTilesPreloadHolder : ContainerFacade
    {
        protected BaseConveyorTilesPreloadHolder(Container container) : base(container)
        {
        }
    }

    public class ConveyorTilesPreloadHolder : BaseConveyorTilesPreloadHolder
    {
        private static readonly string ContainerName = "ConveyorTilesPreloadHolder";

        private BaseConveyorTilesHolder _conveyorTilesHolder;

        public ConveyorTilesPreloadHolder(
            Container container)
            : base(container)
        {
        }

        protected async override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _conveyorTilesHolder = ResolveFromChildContainer<BaseConveyorTilesHolder>();
            await _conveyorTilesHolder.Initialize(token);
        }

        protected override void ActAfterContainerDisposed()
        {
        }

        protected override void ActBeforeContainerDisposed()
        {
            _conveyorTilesHolder.Dispose();
        }

        protected override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            return UniTask.FromResult(
                    Container.Scope(ContainerName, descriptor =>
                    {
                        descriptor.AddSingleton(typeof(ConveyorTileProvider), typeof(BaseConveyorTileProvider));
                        descriptor.AddTransient(typeof(ConveyorTileInstantiator), typeof(LocalAssetInstantiator<ConveyorTileView>));
                        descriptor.AddTransient(typeof(ConveyorTilesHolder), typeof(BaseConveyorTilesHolder));
                    })
                );
        }
    }
}