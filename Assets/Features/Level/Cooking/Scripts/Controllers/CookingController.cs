using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public class CookingController : ResourcefulController
    {
        private readonly CookingViewProvider _cookingViewProvider;
        private readonly CookingUiProvider _cookingUiProvider;
        private readonly ICookingStageExternalEvents _cookingStageExternalEvents;
        private readonly CookingControllerData _data;

        private CookingView _view;
        private CookingUiView _uiView;

        public CookingController(
            CookingViewProvider cookingViewProvider,
            CookingUiProvider cookingUiProvider,
            ICookingStageExternalEvents cookingStageExternalEvents)
        {
            _cookingViewProvider = cookingViewProvider;
            _cookingUiProvider = cookingUiProvider;
            _cookingStageExternalEvents = cookingStageExternalEvents;

            _data = new CookingControllerData();
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefabs();

            _cookingStageExternalEvents.ShowRequest += OnShowWindowRequest;
        }

        public override void Dispose()
        {
            _cookingStageExternalEvents.ShowRequest -= OnShowWindowRequest;

            base.Dispose();
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

        private async UniTask LoadPrefabs()
        {
            await UniTask.WhenAll(LoadPrefab(), LoadUiPrefab());

            ShowWindow(false);
        }

        private void OnShowWindowRequest(bool shouldShow)
        {
            ShowWindow(shouldShow);
        }

        private void ShowWindow(bool shouldShow)
        {
            _view.Toggle(shouldShow);
            _uiView.Toggle(shouldShow);

            if (shouldShow)
            {
                _uiView.OnBackButtonClick += OnBackButtonClickHappen;
            }
            else
            {
                _uiView.OnBackButtonClick -= OnBackButtonClickHappen;
            }

            HideAllSubViews();

            switch (_data.DishType)
            {
                case DishType.None:
                    ShowDishMenu();
                    break;

                case DishType.Nigiri:
                    break;

                case DishType.Maki:
                    ShowCookingMaki();
                    break;

                default:
                    break;
            }
        }

        private void HideAllSubViews()
        {
            _uiView.CookingTypeMenuUiView.Toggle(false);
        }

        private void ShowDishMenu()
        {
            var cookingTypeMenuView = _uiView.CookingTypeMenuUiView;

            cookingTypeMenuView.Toggle(true);

            cookingTypeMenuView.OnButtonClick += OnCookingTypeClick;
        }

        private void OnCookingTypeClick(DishType type)
        {
            var cookingTypeMenuView = _uiView.CookingTypeMenuUiView;

            cookingTypeMenuView.Toggle(false);
            cookingTypeMenuView.OnButtonClick -= OnCookingTypeClick;

            _data.DishType = type;

            ShowCookingMaki();
        }

        private void ShowCookingMaki()
        {
            var makiView = _view.CookingMakiView;

            makiView.Toggle(true);

            ShowCookingMakiBaseIngridients();
        }

        private void ShowCookingMakiBaseIngridients()
        {
        }

        private void OnBackButtonClickHappen()
        {
            _cookingStageExternalEvents.ReportBackButtonClicked();
        }
    }
}