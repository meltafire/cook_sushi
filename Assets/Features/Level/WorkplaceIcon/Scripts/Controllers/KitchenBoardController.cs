using Assets.Utils.Ui;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardController : BaseKitchenBoardController
    {
        private readonly IKitchenBoardIconEvents _kitchenBoardIconEvents;
        private readonly IButtonView _view;

        public KitchenBoardController(
            IKitchenBoardIconEvents kitchenBoardIconEvents,
            IButtonView view)
        {
            _kitchenBoardIconEvents = kitchenBoardIconEvents;
            _view = view;
        }

        public override UniTask Initialize(CancellationToken token)
        {
            _kitchenBoardIconEvents.ToggleRequest += OnToggleRequested;

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _kitchenBoardIconEvents.ToggleRequest -= OnToggleRequested;
            _view.ButtonPressed -= OnClickHappened;
        }

        private void OnToggleRequested(bool isOn)
        {
            if (isOn)
            {
                _view.ButtonPressed += OnClickHappened;
            }
            else
            {
                _view.ButtonPressed -= OnClickHappened;
            }
        }

        private void OnClickHappened()
        {
            _kitchenBoardIconEvents.ReportButtonClick();
        }
    }

    public abstract class BaseKitchenBoardController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
    }
}