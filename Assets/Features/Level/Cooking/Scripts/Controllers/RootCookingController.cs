using Assets.Features.Level.Cooking.Scripts.Controllers.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.States;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Cooking;
using System.Threading;
using Utils.Controllers;

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
                    descriptor.AddInstance(_uiView, typeof(CookingUiView));
                    descriptor.AddInstance(_uiView.CookingTypeMenuUiView, typeof(CookingTypeMenuUiView));

                    descriptor.AddSingleton(typeof(CookingController), typeof(BaseCookingController), typeof(IStateChanger));

                    descriptor.AddTransient(typeof(DishSelectionState));
                    descriptor.AddTransient(typeof(MakiState));
                    descriptor.AddTransient(typeof(NigiriState));
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
    }
}