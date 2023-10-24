using Assets.Features.Level.Conveyor.Scripts.Providers;
using Assets.Features.Level.Conveyor.Scripts.Views;
using Assets.Utils.AssetProvider;
using Assets.Utils.Controllers;
using Assets.Utils.ReflexIntegration;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils.AssetProvider;

namespace Assets.Features.Level.Conveyor.Scripts.Controllers
{
    public abstract class BaseConveyorTilesHolder : ContainerFacade
    {
        protected BaseConveyorTilesHolder(Container container) : base(container)
        {
        }
    }

    public class ConveyorTilesHolder : BaseConveyorTilesHolder
    {
        private static readonly string ContainerName = "ConveyorTilesHolder";

        private readonly Queue<IControllerWithData<int>> _tileHolders = new Queue<IControllerWithData<int>>();

        private readonly BaseConveyorTileProvider _tileProvider;
        private readonly IConveyorView _conveyorView;

        public ConveyorTilesHolder(
            Container container,
            BaseConveyorTileProvider tileProvider,
            IConveyorView conveyorView)
            : base(container)
        {
            _tileProvider = tileProvider;
            _conveyorView = conveyorView;
        }

        protected async override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            var loading = new Queue<UniTask>();

            for (int i = 0; i < _conveyorView.TileCountTotal; i++)
            {
                var tileHolder = ResolveFromChildContainer<ConveyorTileHolder>();

                loading.Enqueue(tileHolder.Initialize(i, token));

                _tileHolders.Enqueue(tileHolder);
            }

            await UniTask.WhenAll(loading);
        }

        protected override void ActAfterContainerDisposed()
        {
        }

        protected override void ActBeforeContainerDisposed()
        {
            _tileProvider.Unload();

            while (_tileHolders.Count > 0)
            {
                _tileHolders.Dequeue().Dispose();
            }
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            await _tileProvider.Load(token);

            var pointsProvider = GenerateConveyorData(
                _tileProvider.GameObject.GetComponent<IConveyorTileView>().SpriteLength,
                _conveyorView.TopStart.position,
                _conveyorView.BottomStart.position,
                _conveyorView.TileCountTotal,
                _conveyorView.TopTileSize);

            return Container.Scope(ContainerName, descriptor =>
                    {
                        descriptor.AddTransient(typeof(ConveyorTileHolder));
                        descriptor.AddInstance(pointsProvider, typeof(IConveyorPointsProvider));
                    });
        }

        private IConveyorPointsProvider GenerateConveyorData(float tileLength, Vector3 topStart, Vector3 bottomStart, int tileCountTotal, int topTileSize)
        {
            var topEnd = topStart + Vector3.right * tileLength * topTileSize;
            var bottomEnd = bottomStart + Vector3.right * tileLength * (tileCountTotal - topTileSize);

            return new ConveyorGameObjectData(
                topStart,
                topEnd,
                bottomStart,
                bottomEnd);
        }
    }
}