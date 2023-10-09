using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Display;
using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.States;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public class CookingController : ResourcefulController
    {
        private readonly CookingView _view;
        private readonly ICookingUiView _uiView;
        private readonly CookingRecepieUiView _recepieView;
        private readonly ICookingControllerEvents _events;
        private readonly Dictionary<ControllerStatesType, ICookingControllerState> _statesDisctionary;
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly IFactory<CookingMakiRecepieController> _makiRecepieControllerFactory;
        private readonly IFactory<CookingNigiriRecepieController> _nigiriRecepieControllerFactory;
        private readonly IFactory<CookingDisplayMakiRecepieController> _displayMakiRecepieControllerFactory;
        private readonly List<ResourcefulController> _dynamicControllers = new List<ResourcefulController>();

        private CancellationToken _token;

        public CookingController(
            CookingView view,
            ICookingUiView uiView,
            CookingRecepieUiView recepieView,
            ICookingControllerEvents events,
            ILevelDishesTypeProvider levelDishesTypeProvider,
            IFactory<CookingMakiRecepieController> makiRecepieControllerFactory,
            IFactory<CookingNigiriRecepieController> nigiriRecepieControllerFactory,
            IFactory<CookingDisplayMakiRecepieController> displayMakiRecepieControllerFactory,
            IngridientsState ingridientsState,
            RecepieSelectionState recepieSelectionState,
            MakiIngridientsState makiIngridientsState,
            FinalizationState finalizationState)
        {
            _view = view;
            _uiView = uiView;
            _recepieView = recepieView;
            _events = events;
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _makiRecepieControllerFactory = makiRecepieControllerFactory;
            _nigiriRecepieControllerFactory = nigiriRecepieControllerFactory;
            _displayMakiRecepieControllerFactory = displayMakiRecepieControllerFactory;

            _statesDisctionary = new Dictionary<ControllerStatesType, ICookingControllerState>()
            {
                { ControllerStatesType.RecepieSelectionState, recepieSelectionState},
                { ControllerStatesType.IngridientsState, ingridientsState},
                { ControllerStatesType.MakiIngridientsState, makiIngridientsState},
                { ControllerStatesType.FinalizationState, finalizationState},
            };
        }

        public override UniTask Initialzie(CancellationToken token)
        {
            _token = token;

            ShowWindow(false);

            ResetView();

            Start().Forget();
            _events.ShowRequest += OnShowWindowRequest;

            return SpawnRecepieButtons(token);
        }

        public override void Dispose()
        {
            DisposeRecepieButtons();

            _events.ShowRequest -= OnShowWindowRequest;

            base.Dispose();
        }

        private async UniTask Start()
        {
            var stateType = ControllerStatesType.RecepieSelectionState;
            var ingridients = new List<CookingAction>();

            while (!_token.IsCancellationRequested)
            {
                if (stateType == ControllerStatesType.RecepieSelectionState)
                {
                    ingridients.Clear();
                }

                stateType = await _statesDisctionary[stateType].Run(ingridients, _token);
            }
        }

        private void OnShowWindowRequest()
        {
            ShowWindow(true);
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
        }

        private void ResetView()
        {
            HideAllSubViews();
            ToggleBackButton(true);
        }

        private void OnBackButtonClickHappen()
        {
            ShowWindow(false);
            _events.ReportPopupClosed();
        }

        private void ToggleBackButton(bool isOn)
        {
            _uiView.ToggleBackButton(isOn);
        }

        private void HideAllSubViews()
        {
            _recepieView.ShowButtons(false);
        }

        private UniTask SpawnRecepieButtons(CancellationToken token)
        {
            var types = _levelDishesTypeProvider.GetLevelDishTypes();

            if(types.Contains(DishType.Maki))
            {
                _dynamicControllers.Add(_makiRecepieControllerFactory.Create());
                _dynamicControllers.Add(_displayMakiRecepieControllerFactory.Create());
            }

            if(types.Contains(DishType.Nigiri))
            {
                _dynamicControllers.Add(_nigiriRecepieControllerFactory.Create());
            }

            var loadingTasks = new List<UniTask>();

            foreach(var controller in _dynamicControllers)
            {
                loadingTasks.Add(controller.Initialzie(token));
            }

            return UniTask.WhenAll(loadingTasks);
        }

        private void DisposeRecepieButtons()
        {
            foreach(var controller in _dynamicControllers)
            {
                controller.Dispose();
            }
        }
    }
}