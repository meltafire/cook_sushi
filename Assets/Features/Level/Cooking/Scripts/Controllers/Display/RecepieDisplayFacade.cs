using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Actions;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Handler
{
    public class RecepieDisplayFacade : ContainerFacade
    {
        private static readonly string ContainerName = "RecepieDisplayFacade";

        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly IRecepieAccountingDrawSignals _ingridientSelectionExternalEvent;
        private readonly CookingDisplayMakiStartInstantiator _cookingDisplayMakiStartInstantiator;
        private readonly CookingDisplayMakiEndInstantiator _cookingDisplayMakiEndInstantiator;
        private readonly CookingDisplayNigiriInstantiator _cookingDisplayNigiriInstantiator;
        private readonly CookingDisplayMakiWrapInstantiator _displayMakiWrapInstantiator;
        private readonly CookingMakiWrapActionInstantiator _makiWrapActionInstantiator;

        private BaseCookingDisplayMakiRecepieController _displayMakiRecepieController;
        private IController _displayIngridientFacade;
        private BaseCookingDisplayNigiriRecepieController _displayNigiriRecepieController;
        private BaseCookingDisplayMakiWrapController _displayMakiWrapController;
        private BaseCookingMakiWrapActionController _makiWrapActionController;

        public RecepieDisplayFacade(
            Container container,
            ILevelDishesTypeProvider levelDishesTypeProvider,
            IRecepieAccountingDrawSignals ingridientSelectionExternalEvent,
            CookingDisplayMakiStartInstantiator cookingDisplayMakiStartInstantiator,
            CookingDisplayMakiEndInstantiator cookingDisplayMakiEndInstantiator,
            CookingDisplayNigiriInstantiator cookingDisplayNigiriInstantiator,
            CookingDisplayMakiWrapInstantiator displayMakiWrapInstantiator,
            CookingMakiWrapActionInstantiator makiWrapActionInstantiator
            ) : base(container)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _ingridientSelectionExternalEvent = ingridientSelectionExternalEvent;
            _cookingDisplayMakiStartInstantiator = cookingDisplayMakiStartInstantiator;
            _cookingDisplayMakiEndInstantiator = cookingDisplayMakiEndInstantiator;
            _cookingDisplayNigiriInstantiator = cookingDisplayNigiriInstantiator;
            _displayMakiWrapInstantiator = displayMakiWrapInstantiator;
            _makiWrapActionInstantiator = makiWrapActionInstantiator;
        }

        protected override void ActAfterContainerDisposed()
        {
            _cookingDisplayMakiStartInstantiator.Unload();
            _cookingDisplayMakiEndInstantiator.Unload();
            _cookingDisplayNigiriInstantiator.Unload();
            _displayMakiWrapInstantiator.Unload();
            _makiWrapActionInstantiator.Unload();
        }

        protected async override UniTask ActAfterContainerInitialized(CancellationToken token)
        {
            _displayMakiRecepieController = ResolveFromChildContainer<BaseCookingDisplayMakiRecepieController>();
            _displayIngridientFacade = ResolveFromChildContainer<CookingDisplayIngridientsFacade>();
            _displayNigiriRecepieController = ResolveFromChildContainer<BaseCookingDisplayNigiriRecepieController>();
            _displayMakiWrapController = ResolveFromChildContainer<BaseCookingDisplayMakiWrapController>();
            _makiWrapActionController = ResolveFromChildContainer<BaseCookingMakiWrapActionController>();

            await UniTask.WhenAll(
                _displayMakiRecepieController.Initialize(token),
                _displayIngridientFacade.Initialize(token),
                _displayNigiriRecepieController.Initialize(token),
                _displayMakiWrapController.Initialize(token),
                _makiWrapActionController.Initialize(token));

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
        }

        protected override void ActBeforeContainerDisposed()
        {
            _ingridientSelectionExternalEvent.DisplayIngridient -= OnIngridientSelected;

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

            _displayMakiRecepieController.Dispose();
            _displayIngridientFacade.Dispose();
            _displayNigiriRecepieController.Dispose();
            _displayMakiWrapController.Dispose();
            _makiWrapActionController.Dispose();
        }

        protected async override UniTask<Container> GenerateContainer(CancellationToken token)
        {
            var makiStartView = await _cookingDisplayMakiStartInstantiator.Load();
            var makiEndView = await _cookingDisplayMakiEndInstantiator.Load();
            var nigiriStartView = await _cookingDisplayNigiriInstantiator.Load();
            var displayMakiWrap = await _displayMakiWrapInstantiator.Load();
            var makiWrapActionView = await _makiWrapActionInstantiator.Load();

            return Container.Scope(ContainerName, descriptor =>
            {
                descriptor.AddInstance(makiStartView, typeof(ICookingDisplayMakiStartRecepieView));
                descriptor.AddInstance(makiEndView, typeof(ICookingDisplayMakiEndRecepieView));
                descriptor.AddTransient(typeof(CookingDisplayMakiRecepieController), typeof(BaseCookingDisplayMakiRecepieController));

                descriptor.AddTransient(typeof(CookingDisplayIngridientsFacade));

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

        private void OnIngridientSelected(CookingIngridientType type, int count)
        {
            _makiWrapActionController.MoveLast();
        }
    }
}