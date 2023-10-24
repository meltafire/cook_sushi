using Assets.Features.Level.Conveyor.Scripts.Views;
using Assets.Utils.ReflexIntegration;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using System.Threading;
using Utils.AssetProvider;

namespace Assets.Features.Level.Conveyor.Scripts.Controllers
{
    public class ConveyorTileHolder : ContainerWithDataFacade<int>
    {
        private static readonly string ContainerName = "ConveyorTileHolder";

        private readonly LocalAssetInstantiator<ConveyorTileView> _localAssetInstantiator;

        private BaseConveyorTileController _controller;

        public ConveyorTileHolder(
            Container container,
            LocalAssetInstantiator<ConveyorTileView> localAssetInstantiator)
            : base(container)
        {
            _localAssetInstantiator = localAssetInstantiator;
        }

        protected override void ActAfterContainerDisposed()
        {
            _localAssetInstantiator.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(int index, CancellationToken token)
        {
            _controller = ResolveFromChildContainer<BaseConveyorTileController>();

            return _controller.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _controller.Dispose();
        }

        protected override UniTask<Container> GenerateContainer(int index, CancellationToken token)
        {
            var data = new ConveyorTileData(index);
            var view = _localAssetInstantiator.Load();

            view.gameObject.SetActive(true);

            return UniTask.FromResult(
                Container.Scope(ContainerName, descriptor =>
                {
                    descriptor.AddInstance(view, typeof(IConveyorTileView));
                    descriptor.AddSingleton(typeof(ConveyorTileController), typeof(BaseConveyorTileController));
                    descriptor.AddInstance(data, typeof(ConveyorTileData));
                }));
        }
    }
}