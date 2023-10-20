using Assets.Features.Level.Cooking.Scripts.Controllers.Recepies;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Handler;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Pools;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.States;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Cooking;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers
{
    public class RootCookingController : IController
    {
        private static readonly string RootCookingControllerName = "RootCookingControllerContainer";

        private readonly CookingViewInstantiator _cookingViewProvider;
        private readonly CookingUiInstantiator _cookingUiProvider;
        private readonly Container _container;

        private Container _childContainer;
        private BaseCookingController _controller;

        public RootCookingController(CookingViewInstantiator cookingViewProvider, CookingUiInstantiator cookingUiProvider, Container container)
        {
            _cookingViewProvider = cookingViewProvider;
            _cookingUiProvider = cookingUiProvider;
            _container = container;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            var (view, uiView) = await UniTask.WhenAll(LoadPrefab(), LoadUiPrefab());

            _childContainer = _container.Scope(RootCookingControllerName, descriptor =>
                {
                    descriptor.AddInstance(view, typeof(CookingView));
                    descriptor.AddInstance(uiView,
                        typeof(CookingUiView)
                        );

                    descriptor.AddSingleton(typeof(CookingController),
                        typeof(BaseCookingController),
                        typeof(IRecepieParentTransformProvider),
                        typeof(IIngridientsParentTransformProvider),
                        typeof(IIngridientsDispalyParentTransformProvider),
                        typeof(ICookingControllerGeneralButtonsProvider),
                        typeof(ICookingControllerRecepieToggleProvider),
                        typeof(ICookingControllerIngridentsToggleProvider)
                        );

                    descriptor.AddTransient(typeof(RecepieSelectionState));
                    descriptor.AddTransient(typeof(IngridientsState));
                    descriptor.AddTransient(typeof(FinalizationState));

                    IsntallCookingIngredients(descriptor);
                });

            _controller = _childContainer.Resolve<BaseCookingController>();

            await _controller.Initialize(token);
        }

        public void Dispose()
        {
            _controller.Dispose();

            _childContainer.Dispose();
            _childContainer = null;
        }

        private UniTask<CookingView> LoadPrefab()
        {
            return _cookingViewProvider.Load();
        }

        private UniTask<CookingUiView> LoadUiPrefab()
        {
            return _cookingUiProvider.Load();
        }

        private void IsntallCookingIngredients(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(RecepieAccounting), typeof(IRecepieAccountingDrawSignals), typeof(IRecepieAccounting));

            descriptor.AddSingleton(typeof(RecipeSelectionEvents),
            typeof(IRecipeSelectionEvents),
            typeof(IRecipeSelectionExternalEvents)
            );

            descriptor.AddTransient(typeof(CookingRecepiesFacade), typeof(BaseCookingRecepiesFacade));
            descriptor.AddTransient(typeof(CookingMakiRecepieInstantiator));
            descriptor.AddTransient(typeof(CookingNigiriRecepieInstantiator));

            descriptor.AddTransient(typeof(CookingIngridientAssetInstantiator));
            descriptor.AddTransient(typeof(DisplayIngridientsControllerPool));

            descriptor.AddTransient(typeof(RecepieDisplayFacade));
            descriptor.AddTransient(typeof(CookingDisplayMakiStartInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayMakiEndInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayNigiriInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayIngridientInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayMakiWrapInstantiator));

            descriptor.AddTransient(typeof(CookingMakiWrapActionInstantiator));
        }
    }
}