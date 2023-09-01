using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Level.Common.Controllers;
using Sushi.Level.Installer;
using Sushi.Menu.Controllers;
using Sushi.Menu.Installer;
using System;
using System.Threading;
using Utils.Controllers;

namespace Sushi.App
{
    public class RootAppController : Controller, IStartable
    {
        private static readonly string MenuContainerName = "MenuContainer";
        private static readonly string LevelContainerName = "LevelContainer";

        private readonly CancellationToken _cancellationToken;
        private readonly Container _container;
        private readonly MenuInstaller _menuInstaller;
        private readonly LevelInstaller _levelInstaller;
        private readonly AppControllerData _data;

        private Container _childContainer;

        public RootAppController(
            CancellationToken cancellationToken,
            Container container,
            MenuInstaller menuInstaller,
            LevelInstaller levelInstaller,
            AppControllerData data)
        {
            _cancellationToken = cancellationToken;
            _container = container;
            _menuInstaller = menuInstaller;
            _levelInstaller = levelInstaller;
            _data = data;
        }

        public async void Start()
        {
            await Run(_cancellationToken);
        }

        protected async override UniTask Run(CancellationToken token)
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

        private UniTask RunNextFeature(CancellationToken token)
        {
            switch (_data.ActionType)
            {
                case AppActionType.Menu:
                    _childContainer = _container.Scope(MenuContainerName, descriptor =>  
                        {
                            _menuInstaller.InstallBindings(descriptor);
                        });

                    return RunChild(_childContainer.Resolve<IFactory<RootMenuController>>(), token);

                case AppActionType.Level:

                    _childContainer = _container.Scope(LevelContainerName, descriptor =>  
                        {
                            _levelInstaller.InstallBindings(descriptor);
                        });

                    return RunChild(_childContainer.Resolve<IFactory<RootLevelController>>(), token);

                case AppActionType.Quit:

                    return UniTask.CompletedTask;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnBubbleEventHappen(ControllerEvent controllerEvent)
        {
            if (controllerEvent is RootAppEvent)
            {
                var rootAppEvent = (RootAppEvent)controllerEvent;

                _data.ActionType = rootAppEvent.AppActionType;
            }
        }
    }
}
