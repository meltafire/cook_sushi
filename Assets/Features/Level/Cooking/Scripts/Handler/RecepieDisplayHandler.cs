using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Pools;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieDisplayHandler
    {
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly IFactory<CookingDisplayMakiRecepieController> _displayMakiRecepieControllerFactory;
        private readonly IFactory<CookingDisplayNigiriRecepieController> _displayNigiriRecepieControllerFactory;
        private readonly IFactory<CookingMakiWrapActionController> _cookingMakiWrapActionControllerFactory;
        private readonly IFactory<CookingDisplayMakiWrapController> _cookingDisplayMakiWrapControllerFactory;
        private readonly DisplayIngridientsControllerPool _ingridientsControllerPool;
        private readonly IRecepieAccountingDrawSignals _ingridientSelectionExternalEvent;
        private readonly Stack<CookingDisplayIngridientController> _ingridientDisplayControllers = new Stack<CookingDisplayIngridientController>();

        private CookingDisplayMakiRecepieController _displayMakiController;
        private CookingDisplayNigiriRecepieController _displayNigiriRecepieController;
        private CookingMakiWrapActionController _wrapMakiController;
        private CookingDisplayMakiWrapController _displayWrapMakiController;
        private CancellationToken _token;

        public RecepieDisplayHandler(
            ILevelDishesTypeProvider levelDishesTypeProvider,
            IFactory<CookingDisplayMakiRecepieController> displayMakiRecepieControllerFactory,
            IFactory<CookingDisplayNigiriRecepieController> displayNigiriRecepieControllerFactory,
            IFactory<CookingMakiWrapActionController> cookingMakiWrapActionControllerFactory,
            IFactory<CookingDisplayMakiWrapController> cookingDisplayMakiWrapControllerFactory,
            DisplayIngridientsControllerPool ingridientsControllerPool,
            IRecepieAccountingDrawSignals ingridientSelectionExternalEvent)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _displayMakiRecepieControllerFactory = displayMakiRecepieControllerFactory;
            _displayNigiriRecepieControllerFactory = displayNigiriRecepieControllerFactory;
            _cookingMakiWrapActionControllerFactory = cookingMakiWrapActionControllerFactory;
            _cookingDisplayMakiWrapControllerFactory = cookingDisplayMakiWrapControllerFactory;
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
                _displayMakiController = _displayMakiRecepieControllerFactory.Create();
                tasks.Add(_displayMakiController.Initialzie(token));

                _wrapMakiController = _cookingMakiWrapActionControllerFactory.Create();
                tasks.Add(_wrapMakiController.Initialzie(token));

                _displayWrapMakiController = _cookingDisplayMakiWrapControllerFactory.Create();
                tasks.Add(_displayWrapMakiController.Initialzie(token));

                _ingridientSelectionExternalEvent.DisplayMakiRecepie += OnDisplayMakiRecepie;
                _ingridientSelectionExternalEvent.DisplayWrapMaki += OnDisplayWrapMaki;
            }

            if (types.Contains(DishType.Nigiri))
            {
                _displayNigiriRecepieController = _displayNigiriRecepieControllerFactory.Create();
                tasks.Add(_displayNigiriRecepieController.Initialzie(token));

                _ingridientSelectionExternalEvent.DisplayNigiriRecepie += OnDisplayNigiriRecepie;
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

            var types = _levelDishesTypeProvider.GetLevelDishTypes();
            if (types.Contains(DishType.Maki))
            {
                _ingridientSelectionExternalEvent.DisplayMakiRecepie -= OnDisplayMakiRecepie;
                _ingridientSelectionExternalEvent.DisplayWrapMaki -= OnDisplayWrapMaki;

                _displayMakiController.Dispose();
                _wrapMakiController.Dispose();
                _displayWrapMakiController.Dispose();
            }

            if (types.Contains(DishType.Nigiri))
            {
                _ingridientSelectionExternalEvent.DisplayNigiriRecepie += OnDisplayNigiriRecepie;

                _displayNigiriRecepieController.Dispose();
            }

            _ingridientsControllerPool.Dispose();
        }

        private void OnDisplayNigiriRecepie(bool isOn)
        {
            _displayNigiriRecepieController.Show(isOn);
        }

        private void OnDisplayWrapMaki(bool isOn)
        {
            _displayWrapMakiController.Show(isOn);
            _wrapMakiController.Show(!isOn);
        }

        private void OnDisplayMakiRecepie(bool isOn)
        {
            _displayMakiController.Show(isOn);
            _wrapMakiController.Show(isOn);
        }

        private async void OnIngridientSelected(CookingIngridientType type, int count)
        {
            var controller = await _ingridientsControllerPool.Get(_token);

            controller.Show(type, count);

            _ingridientDisplayControllers.Push(controller);

            _wrapMakiController.MoveLast();
        }

        private void OnHideIngridient()
        {
            var controller = _ingridientDisplayControllers.Pop();

            controller.Hide();

            _ingridientsControllerPool.Release(controller);
        }
    }
}