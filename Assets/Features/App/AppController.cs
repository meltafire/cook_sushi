using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Level.Common.Controllers;
using Sushi.Menu.Controllers;
using System.Threading;
using System;
using Utils.Controllers;
using Sushi.Level.Installer;
using Sushi.Menu.Installer;
using Reflex.Core;

namespace Sushi.App
{
    public class AppController : Controller
    {
        private static readonly string MenuContainerName = "MenuContainer";
        private static readonly string LevelContainerName = "LevelContainer";

        private readonly Container _container;
        private readonly MenuInstaller _menuInstaller;
        private readonly LevelInstaller _levelInstaller;
        private readonly AppControllerData _data;

        private Container _childContainer;

        public AppController(
            Container container,
            MenuInstaller menuInstaller,
            LevelInstaller levelInstaller,
            AppControllerData data)
        {
            _container = container;
            _menuInstaller = menuInstaller;
            _levelInstaller = levelInstaller;
            _data = data;
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

                    return _childContainer.Resolve<IFactory<MenuEntryPointController>>().Create().RunChild(this, token);

                case AppActionType.Level:

                    _childContainer = _container.Scope(LevelContainerName, descriptor =>  
                        {
                            _levelInstaller.InstallBindings(descriptor);
                        });

                    return _childContainer.Resolve<IFactory<LevelEntryPointController>>().Create().RunChild(this, token);

                case AppActionType.Quit:

                    return UniTask.CompletedTask;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void HandleBubbleEvent(ControllerEvent controllerEvent)
        {
            if (controllerEvent is RootAppEvent)
            {
                var rootAppEvent = (RootAppEvent)controllerEvent;

                _data.ActionType = rootAppEvent.AppActionType;
            }
        }
    }
}