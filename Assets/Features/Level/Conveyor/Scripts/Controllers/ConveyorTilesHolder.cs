using Assets.Features.Level.Conveyor.Scripts.Providers;
using Assets.Utils.Controllers;
using Assets.Utils.ReflexIntegration;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Views;
using System.Collections.Generic;
using System.Threading;
using Utils.AddressablesLoader;
using Utils.Controllers;

namespace Assets.Features.Level.Conveyor.Scripts.Controllers
{
    public abstract class BaseConveyorsTileHolder : ContainerWithDataFacade<int>
    {
        protected BaseConveyorsTileHolder(Container container) : base(container)
        {
        }
    }

    public class ConveyorTilesHolder : BaseConveyorsTileHolder
    {
        private static readonly string ContainerName = "ConveyorTilesHolder";

        private readonly Queue<IControllerWithData<int>> _tileHolders = new Queue<IControllerWithData<int>>();

        public ConveyorTilesHolder(
            Container container)
            : base(container)
        {
        }

        protected override UniTask ActAfterContainerInitialized(int count, CancellationToken token)
        {
            var loading = new Queue<UniTask>();

            for (int i = 0; i < count; i++)
            {
                var tileHolder = ResolveFromChildContainer<ConveyorTileHolder>();

                loading.Enqueue(tileHolder.Initialize(i, token));

                _tileHolders.Enqueue(tileHolder);
            }

            return UniTask.WhenAll(loading);
        }

        protected override void ActAfterContainerDisposed()
        {
        }

        protected override void ActBeforeContainerDisposed()
        {
            while (_tileHolders.Count > 0)
            {
                _tileHolders.Dequeue().Dispose();
            }
        }

        protected override UniTask<Container> GenerateContainer(int count, CancellationToken token)
        {
            return UniTask.FromResult(
                    Container.Scope(ContainerName, descriptor =>
                    {
                        descriptor.AddTransient(typeof(ConveyorTileInstantiator), typeof(AssetInstantiator<ConveyorTileView>));
                        descriptor.AddTransient(typeof(ConveyorTileHolder));
                    })
                );
        }
    }
}