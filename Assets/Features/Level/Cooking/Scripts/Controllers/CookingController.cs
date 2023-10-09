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
        private readonly ICookingControllerEvents _events;
        private readonly CookingView _view;
        private readonly ICookingUiView _uiView;
        private readonly CookingRecepieUiView _recepieView;

        private readonly Dictionary<ControllerStatesType, ICookingControllerState> _statesDisctionary;
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly IFactory<CookingMakiRecepieController> _makiRecepieControllerFactory;
        private readonly IFactory<CookingNigiriRecepieController> _nigiriRecepieControllerFactory;
        private readonly IFactory<CookingDisplayMakiRecepieController> _displayMakiRecepieControllerFactory;
        private readonly List<ResourcefulController> _dynamicControllers = new List<ResourcefulController>();

        private CancellationToken _token;

        public CookingController(
            ICookingControllerEvents events,
            CookingView view,
            ICookingUiView uiView,
            CookingRecepieUiView recepieView,
            ILevelDishesTypeProvider levelDishesTypeProvider,
            IFactory<CookingMakiRecepieController> makiRecepieControllerFactory,
            IFactory<CookingNigiriRecepieController> nigiriRecepieControllerFactory,
            IFactory<CookingDisplayMakiRecepieController> displayMakiRecepieControllerFactory,
            IngridientsState ingridientsState,
            RecepieSelectionState recepieSelectionState,
            MakiIngridientsState makiIngridientsState,
            FinalizationState finalizationState)
        {
            _events = events;
            _view = view;
            _uiView = uiView;
            _recepieView = recepieView;
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
            _events.ToggleDoneButton += OnToggleDoneButton;
            _events.ToggleBackButton += OnToggleBackButton;
            _events.ToggleRevertButton += OnToggleRevertButton;

            _uiView.BackButtonView.OnButtonPressed += OnBackButtonClickHappen;
            _uiView.RevertButtonView.OnButtonPressed += OnRevertButtonClickHappen;
            _uiView.DoneButtonView.OnButtonPressed += OnDoneButtonClickHappen;

            return SpawnRecepieButtons(token);
        }

        public override void Dispose()
        {
            DisposeRecepieButtons();

            _events.ShowRequest -= OnShowWindowRequest;
            _events.ToggleDoneButton -= OnToggleDoneButton;
            _events.ToggleBackButton -= OnToggleBackButton;
            _events.ToggleRevertButton -= OnToggleRevertButton;

            _uiView.BackButtonView.OnButtonPressed -= OnBackButtonClickHappen;
            _uiView.RevertButtonView.OnButtonPressed -= OnRevertButtonClickHappen;
            _uiView.DoneButtonView.OnButtonPressed -= OnDoneButtonClickHappen;

            base.Dispose();
        }

        private async UniTask Start()
        {
            var stateType = ControllerStatesType.RecepieSelectionState;
            var ingridients = new Stack<CookingAction>();

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
        }

        private void ResetView()
        {
            HideAllSubViews();
            
            _uiView.BackButtonView.Toggle(true);
            _uiView.DoneButtonView.Toggle(false);
            _uiView.RevertButtonView.Toggle(false);
        }

        private void OnBackButtonClickHappen()
        {
            ShowWindow(false);
            _events.ReportPopupClosed();
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

        private void OnDoneButtonClickHappen()
        {
            _events.ReportDonePressed();
        }

        private void OnRevertButtonClickHappen()
        {
            _events.ReportRevertPressed();
        }

        private void OnToggleRevertButton(bool isOn)
        {
            _uiView.RevertButtonView.Toggle(isOn);
        }

        private void OnToggleBackButton(bool isOn)
        {
            _uiView.BackButtonView.Toggle(isOn);
        }

        private void OnToggleDoneButton(bool isOn)
        {
            _uiView.DoneButtonView.Toggle(isOn);
        }
    }
}