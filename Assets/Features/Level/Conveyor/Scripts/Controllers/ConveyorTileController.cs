using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using System.Threading;
using UnityEngine;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Controllers
{
    public class ConveyorTileController : ResourcefulController
    {
        private readonly IConveyorPointProvider _conveyorPointProvider;
        private readonly ConveyorTileView _view;
        private readonly ConveyorTileData _data;
        private readonly IIdleStageControllerEvents _idleStageControllerEvents;

        public ConveyorTileController(
            IConveyorPointProvider conveyorPointProvider,
            ConveyorTileView view,
            ConveyorTileData data,
            IIdleStageControllerEvents idleStageControllerEvents
            )
        {
            _conveyorPointProvider = conveyorPointProvider;
            _view = view;
            _data = data;
            _idleStageControllerEvents = idleStageControllerEvents;
        }

        public override UniTask Initialzie(CancellationToken token)
        {
            _idleStageControllerEvents.LaunchGameplayRequest += OnLaunchGameplayRequest;

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _view.OnUpdate -= OnUpdateHappened;
            _idleStageControllerEvents.LaunchGameplayRequest -= OnLaunchGameplayRequest;

            base.Dispose();
        }

        private void OnUpdateHappened()
        {
            var newPosition = _view.Position + Vector3.right * Time.deltaTime;

            if (_data.IsTopRow && newPosition.x > _conveyorPointProvider.TopEnd.x)
            {
                var overShoot = newPosition.x - _conveyorPointProvider.TopEnd.x;

                _data.IsTopRow = false;

                newPosition = _conveyorPointProvider.BottomStart + Vector3.right * overShoot;
            }
            else if (!_data.IsTopRow && newPosition.x > _conveyorPointProvider.BottomEnd.x)
            {
                var overShoot = newPosition.x - _conveyorPointProvider.BottomEnd.x;

                _data.IsTopRow = true;

                newPosition = _conveyorPointProvider.TopStart + Vector3.right * overShoot;
            }

            _view.SetPosition(newPosition);
        }

         private void OnLaunchGameplayRequest()
        {
            _view.OnUpdate += OnUpdateHappened;
        }
    }
}