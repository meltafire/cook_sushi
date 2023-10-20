using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Menu
{
    public abstract class BaseLevelMenuController : IController
    {
        public abstract void Dispose();

        public abstract UniTask Initialize(CancellationToken token);
    }

    public class LevelMenuController : BaseLevelMenuController
    {
        private readonly LevelMenuView _levelMenuView;
        private readonly ILevelMenuEvents _levelMenuEvents;

        public LevelMenuController(LevelMenuView levelMenuView, ILevelMenuEvents levelMenuEvents)
        {
            _levelMenuView = levelMenuView;
            _levelMenuEvents = levelMenuEvents;
        }

        public override UniTask Initialize(CancellationToken token)
        {
            _levelMenuEvents.ToggleRequest += OnToggleRequested;

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _levelMenuEvents.ToggleRequest -= OnToggleRequested;
            _levelMenuView.OnButtonClick -= OnButtonClickHappened;
        }

        private void OnToggleRequested(bool isOn)
        {
            if (isOn)
            {
                _levelMenuView.OnButtonClick += OnButtonClickHappened;
            }
            else
            {
                _levelMenuView.OnButtonClick -= OnButtonClickHappened;
            }
        }

        private void OnButtonClickHappened()
        {
            _levelMenuEvents.ReportButtonClick();
        }
    }
}