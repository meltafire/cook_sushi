using Assets.Features.Menu.Scripts.Controllers;
using Assets.Features.Menu.Scripts.Events;
using Assets.Features.Menu.Scripts.Views;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Menu.Controllers
{
    public abstract class BaseMenuFacade : ContainerFacade
    {
        public event Action StageCompleted;

        protected BaseMenuFacade(Container container) : base(container)
        {
        }

        protected void ReportStageCompleted()
        {
            StageCompleted?.Invoke();
        }
    }

    public class MenuFacade : BaseMenuFacade
    {
        private static string ContainerName = "MenuFacade";

        private readonly MenuViewProvider _menuViewProvider;
        private readonly IMenuControllerExternalEvents _menuControllerExternalEvents;

        private IController _menuController;

        public MenuFacade(
            Container container,
            MenuViewProvider menuViewProvider,
            IMenuControllerExternalEvents menuControllerExternalEvents)
            : base(container)
        {
            _menuViewProvider = menuViewProvider;
            _menuControllerExternalEvents = menuControllerExternalEvents;
        }

        protected override async UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var view = await _menuViewProvider.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(view, typeof(IMenuView));
                descriptor.AddTransient(typeof(MenuController), typeof(BaseMenuController));
            });
        }

        protected async override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _menuController = ResolveFromChildContainer<BaseMenuController>();

            await _menuController.Initialize(token);

            _menuControllerExternalEvents.ButtonClicked += OnButtonClick;
        }

        protected override void ActBeforeContainerDisposed()
        {
            _menuController.Dispose();

            _menuControllerExternalEvents.ButtonClicked -= OnButtonClick;
        }

        protected override void ActAfterContainerDisposed()
        {
            _menuViewProvider.Unload();
        }

        private void OnButtonClick()
        {
            ReportStageCompleted();
        }
    }
}