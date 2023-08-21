using Cysharp.Threading.Tasks;
using Sushi.App.Data;
using Sushi.App.Events;
using Sushi.Menu.Installer;
using System;
using System.Threading;
using UnityEngine;
using Utils.Controllers;
using VContainer.Unity;

namespace Sushi.App
{
    public class RootAppController : Controller, IAsyncStartable
    {
        private readonly LifetimeScope _currentScope;
        private readonly MenuInstaller _menuInstaller;
        private readonly AppControllerData _data;
        private readonly IAppEventProvider _appEventProvider;

        private UniTaskCompletionSource _rootStateCompletionSource;
        private LifetimeScope _childScope;

        public RootAppController(
            LifetimeScope currentScope,
            MenuInstaller menuInstaller,
            AppControllerData data,
            IAppEventProvider appEventProvider)
        {
            _currentScope = currentScope;
            _menuInstaller = menuInstaller;
            _data = data;
            _appEventProvider = appEventProvider;
        }

        public UniTask StartAsync(CancellationToken token)
        {
            Subscribe();

            return Run(token);
        }

        protected async override UniTask Run(CancellationToken token)
        {
            while (_data.ActionType != AppActionType.Quit)
            {
                _rootStateCompletionSource = new UniTaskCompletionSource();

                _childScope = LaunchNextFeature();

                await _rootStateCompletionSource.Task;

                _childScope.Dispose();

                if (token.IsCancellationRequested)
                {
                    _data.ActionType = AppActionType.Quit;
                }
            }

            Unsubscribe();
        }

        private LifetimeScope LaunchNextFeature()
        {
            switch (_data.ActionType)
            {
                case AppActionType.Menu:

                    return _currentScope.CreateChild(_menuInstaller);

                case AppActionType.Level:

                    Debug.Log("Level");

                    return null;

                case AppActionType.Quit:

                    return null;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Subscribe()
        {
            _appEventProvider.OnFeatureWorkCompletion += OnFeatureWorkCompletion;
        }

        private void Unsubscribe()
        {
            _appEventProvider.OnFeatureWorkCompletion -= OnFeatureWorkCompletion;
        }

        private void OnFeatureWorkCompletion(AppActionType actionType)
        {
            _data.ActionType = actionType;

            _rootStateCompletionSource.TrySetResult();
        }
    }
}
