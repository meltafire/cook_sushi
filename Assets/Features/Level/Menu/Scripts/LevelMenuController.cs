using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Menu
{
    public class LevelMenuController : ResourcefulController
    {
        private readonly LevelMenuProvider _levelMenuProvider;
        private readonly ILevelMenuEvents _levelMenuEvents;

        private LevelMenuView _view;

        public LevelMenuController(LevelMenuProvider levelMenuProvider, ILevelMenuEvents levelMenuEvents)
        {
            _levelMenuProvider = levelMenuProvider;
            _levelMenuEvents = levelMenuEvents;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            _levelMenuEvents.ToggleRequest += OnToggleRequested;

            await LoadLevelMenuPrefab();
        }

        private void OnToggleRequested(bool isOn)
        {
            if (isOn)
            {
                _view.OnButtonClick += OnButtonClickHappened;
            }
            else
            {
                _view.OnButtonClick -= OnButtonClickHappened;
            }
        }

        public override void Dispose()
        {
            _levelMenuEvents.ToggleRequest -= OnToggleRequested;
            _view.OnButtonClick -= OnButtonClickHappened;

            base.Dispose();
        }

        private async UniTask LoadLevelMenuPrefab()
        {
            AttachResource(_levelMenuProvider);

            _view = await _levelMenuProvider.Load();
        }

        private void OnButtonClickHappened()
        {
            _levelMenuEvents.ReportButtonClick();
        }
    }
}