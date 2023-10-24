using Assets.Features.Menu.Scripts.Controllers;
using Assets.Features.Menu.Scripts.Events;
using Assets.Features.Menu.Scripts.Views;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Sushi.Menu.Controllers
{
    public class MenuController : BaseMenuController
    {
        private readonly IMenuView _view;
        private readonly IMenuControllerEvents _events;

        public MenuController(IMenuView view, IMenuControllerEvents events)
        {
            _view = view;
            _events = events;
        }

        public override UniTask Initialize(CancellationToken token)
        {
            _view.OnButtonPressed += OnButtonPressedHappened;

            _view.SetActive(true);

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _view.OnButtonPressed -= OnButtonPressedHappened;
        }

        private void OnButtonPressedHappened()
        {
            _events.ReportButtonPressed();
        }
    }
}