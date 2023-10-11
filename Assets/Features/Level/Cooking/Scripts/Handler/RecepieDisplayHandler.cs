using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Pools;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieDisplayHandler
    {
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly IFactory<CookingDisplayMakiRecepieController> _displayMakiRecepieControllerFactory;
        private readonly DisplayIngridientsControllerPool _ingridientsControllerPool;
        private readonly IRecepieAccountingDrawSignals _ingridientSelectionExternalEvent;
        private readonly Stack<CookingDisplayIngridientController> _ingridientDisplayControllers = new Stack<CookingDisplayIngridientController>();
        private readonly List<ResourcefulController> _recepieDisplayControllers = new List<ResourcefulController>();

        private CancellationToken _token;

        public RecepieDisplayHandler(
            ILevelDishesTypeProvider levelDishesTypeProvider,
            IFactory<CookingDisplayMakiRecepieController> displayMakiRecepieControllerFactory,
            DisplayIngridientsControllerPool ingridientsControllerPool,
            IRecepieAccountingDrawSignals ingridientSelectionExternalEvent)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _displayMakiRecepieControllerFactory = displayMakiRecepieControllerFactory;
            _ingridientsControllerPool = ingridientsControllerPool;
            _ingridientSelectionExternalEvent = ingridientSelectionExternalEvent;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            _token = token;

            var tasks = new List<UniTask>();

            var types = _levelDishesTypeProvider.GetLevelDishTypes();
            if (types.Contains(DishType.Maki))
            {
                var displayMakiRecepieController = _displayMakiRecepieControllerFactory.Create();

                _recepieDisplayControllers.Add(displayMakiRecepieController);

                tasks.Add(displayMakiRecepieController.Initialzie(token));
            }

            tasks.Add(_ingridientsControllerPool.Initialize(token));

            await UniTask.WhenAll(tasks);

            _ingridientSelectionExternalEvent.DisplayIngridient += OnIngridientSelected;
            _ingridientSelectionExternalEvent.HideIngridient += OnHideIngridient;
        }

        public void Dispose()
        {
            _ingridientSelectionExternalEvent.DisplayIngridient -= OnIngridientSelected;
            _ingridientSelectionExternalEvent.HideIngridient -= OnHideIngridient;

            while (_ingridientDisplayControllers.Count == 0)
            {
                var controller = _ingridientDisplayControllers.Pop();

                _ingridientsControllerPool.Release(controller);
            }

            _ingridientsControllerPool.Dispose();
        }

        private async void OnIngridientSelected(CookingIngridientType type, int count)
        {
            var controller = await _ingridientsControllerPool.Get(_token);

            controller.Show(type, count);

            _ingridientDisplayControllers.Push(controller);
        }

        private void OnHideIngridient()
        {
            var controller = _ingridientDisplayControllers.Pop();

            controller.Hide();

            _ingridientsControllerPool.Release(controller);
        }
    }
}