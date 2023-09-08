using Cysharp.Threading.Tasks;
using Sushi.Level.Common.Events;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Menu
{
    public class LevelMenuController : Controller
    {
        private readonly LevelMenuProvider _levelMenuProvider;

        private LevelMenuView _view;
        private UniTaskCompletionSource _completionSource;

        public LevelMenuController(LevelMenuProvider levelMenuProvider)
        {
            _levelMenuProvider = levelMenuProvider;
        }

        protected override async UniTask Run(CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();

            token.Register(OnCanceltaionRequested);

            await LoadConveyorPrefab();

            SubscribeToView();

            ReportReady();

            await _completionSource.Task;

            UnsubscribeFromView();
        }

        private async UniTask LoadConveyorPrefab()
        {
            AttachResource(_levelMenuProvider);

            _view = await _levelMenuProvider.Load();
        }

        private void SubscribeToView()
        {
            _view.OnButtonClick += OnButtonClickHappened;
        }

        private void UnsubscribeFromView()
        {
            _view.OnButtonClick -= OnButtonClickHappened;
        }

        private void OnButtonClickHappened()
        {
            _completionSource.TrySetResult();
        }

        private void OnCanceltaionRequested()
        {
            _completionSource.TrySetResult();
        }

        private void ReportReady()
        {
            InvokeBubbleEvent(new LevelFeatureLoadedEvent());
        }
    }
}
