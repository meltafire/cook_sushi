using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
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
using Utils.Controllers.ReflexIntegration;

namespace Assets.Features.Level.Cooking.Scripts.Controllers
{
    public class RootCookingController : ResourcefulController
    {
        private static readonly string RootCookingControllerName = "RootCookingControllerContainer";

        private readonly CookingViewProvider _cookingViewProvider;
        private readonly CookingUiProvider _cookingUiProvider;
        private readonly Container _container;

        private Container _childContainer;
        private CookingView _view;
        private CookingUiView _uiView;
        private BaseCookingController _controller;

        public RootCookingController(CookingViewProvider cookingViewProvider, CookingUiProvider cookingUiProvider, Container container)
        {
            _cookingViewProvider = cookingViewProvider;
            _cookingUiProvider = cookingUiProvider;
            _container = container;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefabs();

            _childContainer = _container.Scope(RootCookingControllerName, descriptor =>
                {
                    descriptor.AddInstance(_view, typeof(CookingView));
                    descriptor.AddInstance(_uiView,
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
                    descriptor.AddTransient(typeof(MakiIngridientsState));
                    descriptor.AddTransient(typeof(IngridientsState));
                    descriptor.AddTransient(typeof(FinalizationState));

                    IsntallCookingIngredients(descriptor);
                });

            _controller = _childContainer.Resolve<BaseCookingController>();

            await _controller.Initialzie(token);
        }

        public override void Dispose()
        {
            _controller.Dispose();

            _childContainer.Dispose();
            _childContainer = null;

            base.Dispose();
        }

        private async UniTask LoadPrefabs()
        {
            await UniTask.WhenAll(LoadPrefab(), LoadUiPrefab());
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_cookingViewProvider);

            _view = await _cookingViewProvider.Load();
        }

        private async UniTask LoadUiPrefab()
        {
            AttachResource(_cookingUiProvider);

            _uiView = await _cookingUiProvider.Instantiate();
        }

        private void IsntallCookingIngredients(ContainerDescriptor descriptor)
        {
            descriptor.AddSingleton(typeof(RecepieAccounting), typeof(IRecepieAccountingDrawSignals), typeof(IRecepieAccounting));

            descriptor.AddSingleton(typeof(RecipeSelectionEvents),
            typeof(IRecipeSelectionEvents),
            typeof(IRecipeSelectionExternalEvents)
            );

            descriptor.RegisterController<CookingMakiRecepieController>();
            descriptor.RegisterController<CookingNigiriRecepieController>();

            descriptor.AddTransient(typeof(CookingMakiRecepieAssetInstantiator));
            descriptor.AddTransient(typeof(CookingNigiriRecepieAssetProvider));

            descriptor.AddTransient(typeof(CookingIngridientAssetProvider));

            descriptor.AddTransient(typeof(DisplayIngridientsControllerPool));
            descriptor.AddTransient(typeof(RecepieDisplayHandler));
            descriptor.RegisterController<CookingDisplayIngridientController>();
            descriptor.RegisterController<CookingDisplayMakiRecepieController>();
            descriptor.RegisterController<CookingDisplayMakiWrapController>();
            descriptor.AddTransient(typeof(CookingDisplayMakiStartInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayMakiEndInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayIngridientInstantiator));
            descriptor.AddTransient(typeof(CookingDisplayMakiWrapInstantiator));

            descriptor.RegisterController<CookingMakiWrapActionController>();
            descriptor.AddTransient(typeof(CookingMakiWrapActionInstantiator));
        }
    }
}