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
        private readonly CookingUiView _uiView;
        private readonly IRecipeSelectionButtonEvents _recipeSelectionButtonEvents;
        private readonly ICookingControllerEvents _events;
        private readonly Dictionary<ControllerStatesType, ICookingControllerState> _statesDisctionary;

        private CancellationToken _token;

        public CookingController(
            CookingView view,
            CookingUiView uiView,
            IRecipeSelectionButtonEvents recipeSelectionButtonEvents,
            ICookingControllerEvents events,
            IngridientsState ingridientsState,
            RecepieSelectionState recepieSelectionState,
            MakiIngridientsState makiIngridientsState,
            FinalizationState finalizationState)
        {
            _view = view;
            _uiView = uiView;
            _recipeSelectionButtonEvents = recipeSelectionButtonEvents;
            _events = events;

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

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _events.ShowRequest -= OnShowWindowRequest;

            base.Dispose();
        }

        private async UniTask Start()
        {
            var stateType = ControllerStatesType.RecepieSelectionState;
            var ingridients = new List<CookingAction>();

            while(!_token.IsCancellationRequested)
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
            _recipeSelectionButtonEvents.ShowButtons(false);
        }

        public void ShowBackButton(bool isOn)
        {
            _uiView.ToggleBackButton(isOn);
        }
    }
}