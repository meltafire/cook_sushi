using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using Sushi.Level.WorkplaceIcon.Events;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardController : Controller
    {
        private readonly KitchenBoardProvider _kitchenBoardProvider;

        private KitchenBoardView _view;
        private UniTaskCompletionSource _completionSource;

        public KitchenBoardController(KitchenBoardProvider kitchenBoardProvider)
        {
            _kitchenBoardProvider = kitchenBoardProvider;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();
            token.Register(OnCancellationRequested);

            await LoadConveyorPrefab();

            ReportReady();

            await _completionSource.Task;

            _view.OnClick -= OnClickHappened;
        }

        private void OnCancellationRequested()
        {
            _completionSource.TrySetResult();
        }

        private async UniTask LoadConveyorPrefab()
        {
            AttachResource(_kitchenBoardProvider);

            _view = await _kitchenBoardProvider.Load();
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }

        protected override void HandleDivingEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is GameplayLaunchEvent)
            {
                _view.OnClick += OnClickHappened;
            }
        }

        private void OnClickHappened()
        {
            InvokeBubbleEvent(new KitchenBoardClickEvent());
        }
    }
}