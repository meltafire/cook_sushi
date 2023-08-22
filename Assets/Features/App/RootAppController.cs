using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Menu.Controllers;
using Sushi.Menu.Installer;
using System;
using System.Threading;
using UnityEngine;
using Utils.Controllers;
using VContainer;
using VContainer.Unity;

namespace Sushi.App
{
    public class RootAppController : Controller, IAsyncStartable
    {
        private readonly LifetimeScope _currentScope;
        private readonly MenuInstaller _menuInstaller;
        private readonly AppControllerData _data;

        private LifetimeScope _childScope;

        public RootAppController(
            LifetimeScope currentScope,
            MenuInstaller menuInstaller,
            AppControllerData data)
        {
            _currentScope = currentScope;
            _menuInstaller = menuInstaller;
            _data = data;
        }

        public UniTask StartAsync(CancellationToken token)
        {
            return Run(token);
        }

        protected async override UniTask Run(CancellationToken token)
        {
            while (_data.ActionType != AppActionType.Quit)
            {
                await RunNextFeature(token);

                _childScope.Dispose();

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

                    _childScope = _currentScope.CreateChild(_menuInstaller);
                    var controllerFactory = _childScope.Container.Resolve<IFactory<RootMenuController>>();

                    return RunChild(controllerFactory, token);

                case AppActionType.Level:

                    Debug.Log("Level");

                    return UniTask.CompletedTask;

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
