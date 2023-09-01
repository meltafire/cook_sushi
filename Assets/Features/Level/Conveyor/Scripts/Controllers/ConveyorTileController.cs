using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using System;
using System.Threading;
using UnityEngine;
using Utils.Controllers;

namespace Sushi.Level.Conveyor.Controllers
{
    public class ConveyorTileController : Controller
    {
        private readonly IConveyorPointProvider _conveyorPointProvider;
        private readonly ConveyorTileView _view;
        private readonly ConveyorTileData _data;

        private UniTaskCompletionSource _completionSource;

        public ConveyorTileController(
            IConveyorPointProvider conveyorPointProvider,
            ConveyorTileView view,
            ConveyorTileData data
            )
        {
            _conveyorPointProvider = conveyorPointProvider;
            _view = view;
            _data = data;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            AttachResource(_view.gameObject);

            _completionSource = new UniTaskCompletionSource();
            token.Register(OnCancellationRequested);

            _view.OnUpdate += OnUpdateHappened;

            await _completionSource.Task;

            _view.OnUpdate += OnUpdateHappened;
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

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }
    }
}