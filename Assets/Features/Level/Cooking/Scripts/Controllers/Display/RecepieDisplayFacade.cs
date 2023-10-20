using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Pools;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Actions;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieDisplayFacade : ContainerFacade
    {
        private static readonly string ContainerName = "RecepieDisplayFacade";

        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly IRecepieAccountingDrawSignals _ingridientSelectionExternalEvent;
        private readonly DisplayIngridientsControllerPool _ingridientsControllerPool;
        private readonly CookingDisplayMakiStartInstantiator _cookingDisplayMakiStartInstantiator;
        private readonly CookingDisplayMakiEndInstantiator _cookingDisplayMakiEndInstantiator;
        private readonly CookingDisplayIngridientInstantiator _displayIngridientInstantiator;
        private readonly CookingDisplayNigiriInstantiator _cookingDisplayNigiriInstantiator;
        private readonly CookingDisplayMakiWrapInstantiator _displayMakiWrapInstantiator;
        private readonly CookingMakiWrapActionInstantiator _makiWrapActionInstantiator;
        private readonly Stack<CookingDisplayIngridientController> _ingridientDisplayControllers;

        private BaseCookingDisplayMakiRecepieController _displayMakiRecepieController;
        private IController _displayIngridientController;
        private BaseCookingDisplayNigiriRecepieController _displayNigiriRecepieController;
        private BaseCookingDisplayMakiWrapController _displayMakiWrapController;
        private BaseCookingMakiWrapActionController _makiWrapActionController;
        private CancellationToken _token;

        public RecepieDisplayFacade(
            Container container,
            ILevelDishesTypeProvider levelDishesTypeProvider,
            IRecepieAccountingDrawSignals ingridientSelectionExternalEvent,
             DisplayIngridientsControllerPool ingridientsControllerPool,
            CookingDisplayMakiStartInstantiator cookingDisplayMakiStartInstantiator,
            CookingDisplayMakiEndInstantiator cookingDisplayMakiEndInstantiator,
            CookingDisplayIngridientInstantiator displayIngridientInstantiator,
            CookingDisplayNigiriInstantiator cookingDisplayNigiriInstantiator,
            CookingDisplayMakiWrapInstantiator displayMakiWrapInstantiator,
            CookingMakiWrapActionInstantiator makiWrapActionInstantiator
            ) : base(container)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _ingridientSelectionExternalEvent = ingridientSelectionExternalEvent;
            _ingridientsControllerPool = ingridientsControllerPool;
            _cookingDisplayMakiStartInstantiator = cookingDisplayMakiStartInstantiator;
            _cookingDisplayMakiEndInstantiator = cookingDisplayMakiEndInstantiator;
            _displayIngridientInstantiator = displayIngridientInstantiator;
            _cookingDisplayNigiriInstantiator = cookingDisplayNigiriInstantiator;
            _displayMakiWrapInstantiator = displayMakiWrapInstantiator;
            _makiWrapActionInstantiator = makiWrapActionInstantiator;

            _ingridientDisplayControllers = new Stack<CookingDisplayIngridientController>();
        }

        protected override void ActAfterContainerDisposed()
        {
            _cookingDisplayMakiStartInstantiator.Unload();
            _cookingDisplayMakiEndInstantiator.Unload();
            _displayIngridientInstantiator.Unload();
            _cookingDisplayNigiriInstantiator.Unload();
            _displayMakiWrapInstantiator.Unload();
            _makiWrapActionInstantiator.Unload();
        }

        protected async override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _token = token;

            _displayMakiRecepieController = ResolveFromChildContainer<BaseCookingDisplayMakiRecepieController>();
            _displayIngridientController = ResolveFromChildContainer<CookingDisplayIngridientController>();
            _displayNigiriRecepieController = ResolveFromChildContainer<BaseCookingDisplayNigiriRecepieController>();
            _displayMakiWrapController = ResolveFromChildContainer<BaseCookingDisplayMakiWrapController>();
            _makiWrapActionController = ResolveFromChildContainer<BaseCookingMakiWrapActionController>();

            await UniTask.WhenAll(
                _displayMakiRecepieController.Initialize(token),
                _displayIngridientController.Initialize(token),
                _displayNigiriRecepieController.Initialize(token),
                _displayMakiWrapController.Initialize(token),
                _makiWrapActionController.Initialize(token),
                _ingridientsControllerPool.Initialize(token));

            var types = _levelDishesTypeProvider.GetLevelDishTypes();

            if (types.Contains(DishType.Maki))
            {
                _ingridientSelectionExternalEvent.DisplayMakiRecepie += OnDisplayMakiRecepie;
                _ingridientSelectionExternalEvent.DisplayWrapMaki += OnDisplayWrapMaki;
            }

            if (types.Contains(DishType.Nigiri))
            {
                _ingridientSelectionExternalEvent.DisplayNigiriRecepie += OnDisplayNigiriRecepie;
            }

            _ingridientSelectionExternalEvent.DisplayIngridient += OnIngridientSelected;
            _ingridientSelectionExternalEvent.HideIngridient += OnHideIngridient;
        }

        protected override void ActBeforeContainerDisposed()
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
            }

            if (types.Contains(DishType.Nigiri))
            {
                _ingridientSelectionExternalEvent.DisplayNigiriRecepie -= OnDisplayNigiriRecepie;
            }

            _ingridientsControllerPool.Dispose();

            _displayMakiRecepieController.Dispose();
            _displayIngridientController.Dispose();
            _displayNigiriRecepieController.Dispose();
            _displayMakiWrapController.Dispose();
            _makiWrapActionController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var makiStartView = await _cookingDisplayMakiStartInstantiator.Load();
            var makiEndView = await _cookingDisplayMakiEndInstantiator.Load();
            var ingridientView = await _displayIngridientInstantiator.Load();
            var nigiriStartView = await _cookingDisplayNigiriInstantiator.Load();
            var displayMakiWrap = await _displayMakiWrapInstantiator.Load();
            var makiWrapActionView = await _makiWrapActionInstantiator.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(makiStartView, typeof(ICookingDisplayMakiStartRecepieView));
                descriptor.AddInstance(makiEndView, typeof(ICookingDisplayMakiEndRecepieView));
                descriptor.AddTransient(typeof(CookingDisplayMakiRecepieController), typeof(BaseCookingDisplayMakiRecepieController));

                descriptor.AddInstance(ingridientView, typeof(CookingDisplayIngridientView));
                descriptor.AddTransient(typeof(CookingDisplayIngridientController));

                descriptor.AddInstance(nigiriStartView, typeof(ICookingDisplayNigiriRecepieView));
                descriptor.AddTransient(typeof(CookingDisplayNigiriRecepieController), typeof(BaseCookingDisplayNigiriRecepieController));

                descriptor.AddInstance(displayMakiWrap, typeof(ICookingDisplayMakiWrapView));
                descriptor.AddTransient(typeof(CookingDisplayMakiWrapController), typeof(BaseCookingDisplayMakiWrapController));

                descriptor.AddInstance(makiWrapActionView, typeof(ICookingMakiWrapActionView));
                descriptor.AddTransient(typeof(CookingMakiWrapActionController), typeof(BaseCookingMakiWrapActionController));
            });
        }

        private void OnDisplayWrapMaki(bool isOn)
        {
            _displayMakiWrapController.Show(isOn);
            _makiWrapActionController.Show(!isOn);
        }

        private void OnDisplayMakiRecepie(bool isOn)
        {
            _displayMakiRecepieController.Show(isOn);
            _makiWrapActionController.Show(isOn);
        }

        private void OnDisplayNigiriRecepie(bool isOn)
        {
            _displayNigiriRecepieController.Show(isOn);
        }

        private async void OnIngridientSelected(CookingIngridientType type, int count)
        {
            var controller = await _ingridientsControllerPool.Get(_token);

            controller.Show(type, count);

            _ingridientDisplayControllers.Push(controller);

            _makiWrapActionController.MoveLast();
        }

        private void OnHideIngridient()
        {
            var controller = _ingridientDisplayControllers.Pop();

            controller.Hide();

            _ingridientsControllerPool.Release(controller);
        }
    }
}