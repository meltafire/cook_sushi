using Assets.Features.Level.Conveyor.Scripts.Events;
using Assets.Features.Level.Conveyor.Scripts.Views;
using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using System.Threading;
using UnityEngine;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Controllers
{
    public abstract class BaseConveyorTileController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
    }

    public class ConveyorTileController : BaseConveyorTileController
    {
        private readonly IConveyorPointsProvider _conveyorPointsProvider;
        private readonly IConveyorTileView _view;
        private readonly ConveyorTileData _data;
        private readonly IConveyorTileEvents _events;

        public ConveyorTileController(
            IConveyorPointsProvider conveyorPointsProvider,
            IConveyorTileView view,
            ConveyorTileData data,
            IConveyorTileEvents events
            )
        {
            _conveyorPointsProvider = conveyorPointsProvider;
            _view = view;
            _data = data;
            _events = events;
        }

        public override UniTask Initialize(CancellationToken token)
        {
            _events.ToggleMovementRequest += OnToggleMovementRequest;

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _view.OnUpdate -= OnUpdateHappened;
            _events.ToggleMovementRequest -= OnToggleMovementRequest;
        }

        private void OnUpdateHappened()
        {
            var newPosition = _view.Position + Vector3.right * Time.deltaTime;

            if (_data.IsTopRow && newPosition.x > _conveyorPointsProvider.TopEnd.x)
            {
                var overShoot = newPosition.x - _conveyorPointsProvider.TopEnd.x;

                _data.IsTopRow = false;

                newPosition = _conveyorPointsProvider.BottomStart + Vector3.right * overShoot;
            }
            else if (!_data.IsTopRow && newPosition.x > _conveyorPointsProvider.BottomEnd.x)
            {
                var overShoot = newPosition.x - _conveyorPointsProvider.BottomEnd.x;

                _data.IsTopRow = true;

                newPosition = _conveyorPointsProvider.TopStart + Vector3.right * overShoot;
            }

            _view.SetPosition(newPosition);
        }

         private void OnToggleMovementRequest(bool isOn)
        {
            if(isOn)
            {
                _view.OnUpdate += OnUpdateHappened;
            }
            else
            {
                _view.OnUpdate -= OnUpdateHappened;
            }
        }
    }
}