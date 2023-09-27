using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.Level.Common.Controllers;
using Sushi.Menu.Controllers;
using System.Threading;
using System;
using Utils.Controllers;
using Sushi.Level.Installer;
using Sushi.Menu.Installer;
using Reflex.Core;
using Sushi.App.LoadingScreen;
using Assets.Features.Menu.Scripts.Data;

namespace Sushi.App
{
    public class AppController : ILaunchableController
    {
        private static readonly string MenuContainerName = "MenuContainer";
        private static readonly string LevelContainerName = "LevelContainer";

        private readonly Container _container;
        private readonly MenuInstaller _menuInstaller;
        private readonly LevelInstaller _levelInstaller;
        private readonly AppControllerData _data;
        private readonly IFactory<LoadingScreenController> _loadingScreenControllerFactory;
        private readonly ILoadingScreenExternalEvents _loadingScreenExternalEvents;

        private Container _childContainer;
        private LoadingScreenController _loadingScreenController;

        public AppController(
            Container container,
            MenuInstaller menuInstaller,
            LevelInstaller levelInstaller,
            AppControllerData data,
            IFactory<LoadingScreenController> loadingScreenControllerFactory,
            ILoadingScreenExternalEvents loadingScreenExternalEvents)
        {
            _container = container;
            _menuInstaller = menuInstaller;
            _levelInstaller = levelInstaller;
            _data = data;
            _loadingScreenControllerFactory = loadingScreenControllerFactory;
            _loadingScreenExternalEvents = loadingScreenExternalEvents;
        }

        public UniTask Initialzie(CancellationToken token)
        {
            var _loadingScreenController = _loadingScreenControllerFactory.Create();

            return _loadingScreenController.Initialzie(token);
        }

        public async UniTask Launch(CancellationToken token)
        {
            while (_data.ActionType != AppActionType.Quit)
            {
                await RunNextFeature(token);

                _childContainer.Dispose();
                _childContainer = null;

                if (token.IsCancellationRequested)
                {
                    _data.ActionType = AppActionType.Quit;
                }
            }
        }

        public void Dispose()
        {
            _loadingScreenController.Dispose();
        }

        private async UniTask RunNextFeature(CancellationToken token)
        {
            switch (_data.ActionType)
            {
                case AppActionType.Menu:
                    await RunMenu(token);

                    return;

                case AppActionType.Level:
                    await RunLevel(token);

                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask RunMenu(CancellationToken token)
        {
            _childContainer = _container.Scope(MenuContainerName, descriptor =>
                {
                    _menuInstaller.InstallBindings(descriptor);
                });

            var controller = _childContainer.Resolve<IFactory<MenuEntryPointController>>().Create();

            _loadingScreenExternalEvents.Show(true);
            await controller.Initialzie(token);
            _loadingScreenExternalEvents.Show(false);


            var result = await controller.Launch(token);

            _loadingScreenExternalEvents.Show(true);
            controller.Dispose();

            HandleMenuResult(result);
        }

        private async UniTask RunLevel(CancellationToken token)
        {
            _childContainer = _container.Scope(LevelContainerName, descriptor =>
                {
                    _levelInstaller.InstallBindings(descriptor);
                });

            var controller = _childContainer.Resolve<IFactory<LevelEntryPointController>>().Create();

            _loadingScreenExternalEvents.Show(true);
            await controller.Initialzie(token);
            _loadingScreenExternalEvents.Show(false);

            await controller.Launch(token);

            _loadingScreenExternalEvents.Show(true);
            controller.Dispose();

            _data.ActionType = AppActionType.Menu;
        }

        private void HandleMenuResult(MenuResults result)
        {
            if (result == MenuResults.Level)
            {
                _data.ActionType = AppActionType.Level;
            }
            else
            {
                _data.ActionType = AppActionType.Quit;
            }
        }
    }
}