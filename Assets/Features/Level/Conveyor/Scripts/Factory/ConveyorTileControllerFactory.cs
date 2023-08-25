using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Services;
using Sushi.Level.Conveyor.Views;
using UnityEngine;

namespace Sushi.Level.Conveyor.Controllers
{
    public class ConveyorTileControllerFactory
    {
        private readonly ConveyorTilePositionService _positionService;
        private readonly ITileGameObjectDataProvider _tileGameObjectDataProvider;
        private readonly IConveyorPointProvider _conveyorPointProvider;

        public ConveyorTileControllerFactory(
            ConveyorTilePositionService positionService,
            ITileGameObjectDataProvider tileGameObjectDataProvider,
            IConveyorPointProvider conveyorPointProvider)
        {
            _positionService = positionService;
            _tileGameObjectDataProvider = tileGameObjectDataProvider;
            _conveyorPointProvider = conveyorPointProvider;
        }

        public ConveyorTileController Create(int index)
        {
            var isOnTop = _positionService.IsTileOnTopRow(index);

            var data = new ConveyorTileData(isOnTop);

            var position = _positionService.GetPosition(index);

            var view = GameObject.Instantiate(
                _tileGameObjectDataProvider.GameObject,
                position,
                Quaternion.identity,
                _tileGameObjectDataProvider.TilesParentTransform)
                .GetComponent<ConveyorTileView>();

            return new ConveyorTileController(
                _conveyorPointProvider,
                view,
                data);
        }
    }
}