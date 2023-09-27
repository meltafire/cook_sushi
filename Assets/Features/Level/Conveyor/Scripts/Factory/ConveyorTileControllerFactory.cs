using Assets.Features.Level.Conveyor.Scripts.Events;
using Sushi.Level.Conveyor.Controllers;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Factory.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Conveyor.Views;
using UnityEngine;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Factory
{
    public class ConveyorTileControllerFactory : IFactoryWithData<ConveyorTileController, ConveyorTileControllerFactoryData>
    {
        private readonly ConveyorTilePositionService _positionService;
        private readonly ITileGameObjectDataProvider _tileGameObjectDataProvider;
        private readonly IConveyorPointProvider _conveyorPointProvider;
        private readonly IConveyorTileEvents _events;

        public ConveyorTileControllerFactory(
            ConveyorTilePositionService positionService,
            ITileGameObjectDataProvider tileGameObjectDataProvider,
            IConveyorPointProvider conveyorPointProvider,
            IConveyorTileEvents events)
        {
            _positionService = positionService;
            _tileGameObjectDataProvider = tileGameObjectDataProvider;
            _conveyorPointProvider = conveyorPointProvider;
            _events = events;
        }

        public ConveyorTileController Create(ConveyorTileControllerFactoryData data)
        {
            var isOnTop = _positionService.IsTileOnTopRow(data.TileIndex);

            var controllerData = new ConveyorTileData(isOnTop);

            var position = _positionService.GetPosition(data.TileIndex);

            var view = GameObject.Instantiate(
                _tileGameObjectDataProvider.GameObject,
                position,
                Quaternion.identity,
                _tileGameObjectDataProvider.TilesParentTransform)
                .GetComponent<ConveyorTileView>();

            return new ConveyorTileController(
                _conveyorPointProvider,
                view,
                controllerData,
                _events);
        }
    }
}