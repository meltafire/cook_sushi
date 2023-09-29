using Assets.Features.Level.Cooking.Scripts.Controllers.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.States;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public abstract class BaseCookingController : ResourcefulController
    {

    }

    public class CookingController : BaseCookingController, IStateChanger
    {
        private readonly CookingView _view;
        private readonly CookingUiView _uiView;
        private readonly ICookingControllerEvents _events;
        private readonly Container _container;

        private ICookingControllerState _state;
        private CancellationToken _token;

        public CookingController(CookingView view, CookingUiView uiView, ICookingControllerEvents events, Container container)
        {
            _view = view;
            _uiView = uiView;
            _events = events;
            _container = container;
        }

        public override UniTask Initialzie(CancellationToken token)
        {
            _token = token;

            ShowWindow(false);

            ResetView();

           var initialState = _container.Resolve<DishSelectionState>();

            SetState(initialState);
            _events.ShowRequest += OnShowWindowRequest;

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            _events.ShowRequest -= OnShowWindowRequest;

            base.Dispose();
        }

        public void SetState(ICookingControllerState state)
        {
            _state = state;

            _state.Run(_token).Forget();
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
                _events.ToggleBackButton += OnToggleBackButton;
            }
            else
            {
                _uiView.OnBackButtonClick -= OnBackButtonClickHappen;
                _events.ToggleBackButton -= OnToggleBackButton;
            }
        }

        private void ResetView()
        {
            HideAllSubViews();
        }

        private void OnBackButtonClickHappen()
        {
            _events.ReportBackButtonClicked();
        }

        private void OnToggleBackButton(bool isOn)
        {
            _uiView.ToggleBackButton(isOn);
        }

        private void HideAllSubViews()
        {
            _uiView.CookingTypeMenuUiView.Toggle(false);
        }
    }

public interface ICookingStatusProvider
{
    public CookingStatus GetStatus();
    public void Reset();
}



public enum CookingStatus
{
    Exit = 0,
    Done = 1,
    DishType = 2,
    MakiBaseIngridients = 3,
    MakiFillingCount = 4,
    MakiFillingPlacement = 5,
}
}