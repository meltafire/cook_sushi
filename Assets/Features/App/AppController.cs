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
using Assets.Features.App.LoadingScreen.Scripts;

namespace Sushi.App
{
    public interface IAppController : IController
    {
        UniTask Launch(CancellationToken token);
    }

    public class AppController : IAppController
    {
        private static readonly string MenuContainerName = "MenuContainer";
        private static readonly string LevelContainerName = "LevelContainer";

        private readonly Container _container;
        private readonly MenuInstaller _menuInstaller;
        private readonly LevelInstaller _levelInstaller;
        private readonly AppControllerData _data;
        private readonly LoadingScreenFacade _loadingScreenFacade;
        private readonly ILoadingScreenExternalEvents _loadingScreenExternalEvents;

        private Container _childContainer;
        private UniTaskCompletionSource _stageCompletionSource;

        public AppController(
            Container container,
            MenuInstaller menuInstaller,
            LevelInstaller levelInstaller,
            AppControllerData data,
            LoadingScreenFacade loadingScreenFacade,
            ILoadingScreenExternalEvents loadingScreenExternalEvents)
        {
            _container = container;
            _menuInstaller = menuInstaller;
            _levelInstaller = levelInstaller;
            _data = data;
            _loadingScreenFacade = loadingScreenFacade;
            _loadingScreenExternalEvents = loadingScreenExternalEvents;
        }

        public UniTask Initialize(CancellationToken token)
        {
            return _loadingScreenFacade.Initialize(token);
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
            _loadingScreenFacade.Dispose();
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

            var facade = _childContainer.Resolve<BaseMenuFacade>();

            _loadingScreenExternalEvents.Show(true);
            _stageCompletionSource = new UniTaskCompletionSource();

            await facade.Initialize(token);
            _loadingScreenExternalEvents.Show(false);

            facade.StageCompleted += OnMenuStageCompleted;
            await _stageCompletionSource.Task;
            facade.StageCompleted += OnMenuStageCompleted;

            _loadingScreenExternalEvents.Show(true);
            facade.Dispose();

            HandleMenuResult(MenuResults.Level);
        }

        private void OnMenuStageCompleted()
        {
            _stageCompletionSource.TrySetResult();
        }

        private async UniTask RunLevel(CancellationToken token)
        {
            _childContainer = _container.Scope(LevelContainerName, descriptor =>
                {
                    _levelInstaller.InstallBindings(descriptor);
                });

            var controller = _childContainer.Resolve<LevelEntryPointController>();

            _loadingScreenExternalEvents.Show(true);
            await controller.Initialize(token);
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