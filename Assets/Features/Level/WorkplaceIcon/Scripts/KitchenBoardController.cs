using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardController : BaseKitchenBoardController
    {
        private readonly KitchenBoardProvider _kitchenBoardProvider;
        private readonly IKitchenBoardIconEvents _kitchenBoardIconEvents;

        private KitchenBoardView _view;

        public KitchenBoardController(
            KitchenBoardProvider kitchenBoardProvider,
            IKitchenBoardIconEvents kitchenBoardIconEvents)
        {
            _kitchenBoardProvider = kitchenBoardProvider;
            _kitchenBoardIconEvents = kitchenBoardIconEvents;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadConveyorPrefab();

            _kitchenBoardIconEvents.ToggleRequest += OnToggleRequested;
        }

        public override void Dispose()
        {
            _kitchenBoardIconEvents.ToggleRequest -= OnToggleRequested;
            _view.OnClick -= OnClickHappened;

            base.Dispose();
        }

        private void OnToggleRequested(bool isOn)
        {
            if (isOn)
            {
                _view.OnClick += OnClickHappened;
            }
            else
            {
                _view.OnClick -= OnClickHappened;
            }
        }

        private async UniTask LoadConveyorPrefab()
        {
            AttachResource(_kitchenBoardProvider);

            _view = await _kitchenBoardProvider.Load();
        }

        private void OnClickHappened()
        {
            _kitchenBoardIconEvents.ReportButtonClick();
        }
    }

    public abstract class BaseKitchenBoardController : ResourcefulController
    {

    }
}