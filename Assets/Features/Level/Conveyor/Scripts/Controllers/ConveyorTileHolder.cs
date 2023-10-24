using Assets.Features.Level.Conveyor.Scripts.Views;
using Assets.Utils.ReflexIntegration;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Conveyor.Views;
using System.Threading;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Conveyor.Scripts.Controllers
{
    public class ConveyorTileHolder : ContainerWithDataFacade<int>
    {
        private static readonly string ContainerName = "ConveyorTileHolder";

        private readonly ConveyorTilePositionService _positionService;
        private readonly AssetInstantiator<ConveyorTileView> _viewInstantiator;

        private BaseConveyorTileController _controller;

        public ConveyorTileHolder(
            Container container,
            ConveyorTilePositionService positionService,
            AssetInstantiator<ConveyorTileView> viewInstantiator)
            : base(container)
        {
            _positionService = positionService;
            _viewInstantiator = viewInstantiator;
        }

        protected override void ActAfterContainerDisposed()
        {
            _viewInstantiator.Unload();
        }

        protected override UniTask ActAfterContainerInitialized(int index, CancellationToken token)
        {
            _controller = ResolveFromChildContainer<BaseConveyorTileController>();

            _controller.SetPosition(_positionService.GetPosition(index));

            return _controller.Initialize(token);
        }

        protected override void ActBeforeContainerDisposed()
        {
            _controller.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(int index, CancellationToken token)
        {
            var view = await _viewInstantiator.Load();
            var artLength = view.SpriteLength;
            _tileGameObjectData.SetIileGameObject(artLength);

            var isOnTop = _positionService.IsTileOnTopRow(index);
            var mvcData = new ConveyorTileData(isOnTop, index);

            return Container.Scope(ContainerName, descriptor =>
                {
                    descriptor.AddInstance(view, typeof(IConveyorTileView));
                    descriptor.AddSingleton(typeof(ConveyorTileController), typeof(BaseConveyorTileController));
                    descriptor.AddInstance(mvcData, typeof(ConveyorTileData));
                });
        }
    }
}