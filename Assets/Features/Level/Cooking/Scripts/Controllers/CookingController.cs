using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Controllers.Recepies;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler;
using Assets.Features.Level.Cooking.Scripts.States;
using Assets.Features.Level.Cooking.Scripts.Views;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using Utils.Controllers;

namespace Sushi.Level.Cooking
{
    public abstract class BaseCookingController : IController
    {
        public abstract void Dispose();
        public abstract UniTask Initialize(CancellationToken token);
    }

    public class CookingController : BaseCookingController,
        ICookingControllerGeneralButtonsProvider,
        ICookingControllerRecepieToggleProvider,
        ICookingControllerIngridentsToggleProvider
    {
        private static readonly string IngridientContainerName = "Ingridient";

        private readonly Container _container;
        private readonly ICookingControllerEvents _events;
        private readonly CookingView _view;
        private readonly CookingUiView _uiView;
        private readonly BaseCookingRecepiesFacade _cookingRecepiesFacade;
        private readonly Dictionary<ControllerStatesType, ICookingControllerState> _statesDisctionary;
        private readonly ILevelIngridientTypeProvider _levelIngridientTypeProvider;
        private readonly RecepieDisplayFacade _recepieDisplayHandler;
        private readonly IIngridientsParentTransformProvider _ingridientsParentTransformProvider;
        private readonly IRecepieParentTransformProvider _recepieParentTransformProvider;
        private readonly List<IController> _dynamicControllers = new List<IController>();

        private Stack<Container> _ingridientsContainers = new Stack<Container>();

        public event Action RevertPressed;
        public event Action DonePressed;

        public CookingController(
            Container container,
            ICookingControllerEvents events,
            CookingView view,
            CookingUiView uiView,
            BaseCookingRecepiesFacade cookingRecepiesFacade,
            ILevelIngridientTypeProvider levelIngridientTypeProvider,
            RecepieDisplayFacade recepieDisplayHandler,
            IIngridientsParentTransformProvider ingridientsParentTransformProvider,
            IRecepieParentTransformProvider recepieParentTransformProvider)
        {
            _container = container;
            _events = events;
            _view = view;
            _uiView = uiView;
            _cookingRecepiesFacade = cookingRecepiesFacade;
            _levelIngridientTypeProvider = levelIngridientTypeProvider;
            _recepieDisplayHandler = recepieDisplayHandler;
            _ingridientsParentTransformProvider = ingridientsParentTransformProvider;
            _recepieParentTransformProvider = recepieParentTransformProvider;

            _statesDisctionary = new Dictionary<ControllerStatesType, ICookingControllerState>(4);
        }

        public override UniTask Initialize(CancellationToken token)
        {
            _statesDisctionary.Add(ControllerStatesType.RecepieSelectionState, _container.Resolve<RecepieSelectionState>());
            _statesDisctionary.Add(ControllerStatesType.IngridientsState, _container.Resolve<IngridientsState>());
            _statesDisctionary.Add(ControllerStatesType.FinalizationState, _container.Resolve<FinalizationState>());

            ShowWindow(false);

            ResetView();

            Start(token).Forget();

            _events.ShowRequest += OnShowWindowRequest;

            _uiView.BackButtonView.ButtonPressed += OnBackButtonClickHappen;
            _uiView.RevertButtonView.ButtonPressed += OnRevertButtonClickHappen;
            _uiView.DoneButtonView.ButtonPressed += OnDoneButtonClickHappen;

            return UniTask.WhenAll(_recepieDisplayHandler.Initialize(token), SpawnCookingButtons(token));
        }

        public override void Dispose()
        {
            DisposeCookingButtons();

            _events.ShowRequest -= OnShowWindowRequest;

            _uiView.BackButtonView.ButtonPressed -= OnBackButtonClickHappen;
            _uiView.RevertButtonView.ButtonPressed -= OnRevertButtonClickHappen;
            _uiView.DoneButtonView.ButtonPressed -= OnDoneButtonClickHappen;

            _recepieDisplayHandler.Dispose();
        }

        public void ToggleRevert(bool isOn)
        {
            _uiView.RevertButtonView.Toggle(isOn);
        }

        public void ToggleDone(bool isOn)
        {
            _uiView.DoneButtonView.Toggle(isOn);
        }

        public void ToggleBackButton(bool isOn)
        {
            _uiView.BackButtonView.Toggle(isOn);
        }

        public void ToggleRecepieButtons(bool isOn)
        {
            _recepieParentTransformProvider.Toggle(isOn);
        }

        public void ToggleIngridientButtons(bool isOn)
        {
            _ingridientsParentTransformProvider.Toggle(isOn);
        }

        private async UniTask Start(CancellationToken token)
        {
            var stateType = ControllerStatesType.RecepieSelectionState;

            while (!token.IsCancellationRequested)
            {
                stateType = await _statesDisctionary[stateType].Run(token);
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
            ToggleRecepieButtons(false);
            ToggleIngridientButtons(false);
            ToggleRevert(false);
            ToggleDone(false);
            ToggleBackButton(true);
        }

        private void OnBackButtonClickHappen()
        {
            ShowWindow(false);
            _events.ReportPopupClosed();
        }

        private UniTask SpawnCookingButtons(CancellationToken token)
        {
            return UniTask.WhenAll(SpawnRecepieButtons(token), SpawnIngridients(token));
        }

        private UniTask SpawnRecepieButtons(CancellationToken token)
        {
            return _cookingRecepiesFacade.Initialize(token);
        }

        private UniTask SpawnIngridients(CancellationToken token)
        {
            var types = _levelIngridientTypeProvider.GetLevelIngridients();
            var loadingTasks = new List<UniTask>();

            foreach (var type in types)
            {
                var childContainer = _container.Scope(IngridientContainerName + type, descriptor =>
                {
                    var data = new CookingIngridientControllerData(type, 0);

                    descriptor.AddInstance(data, typeof(CookingIngridientControllerData));
                    descriptor.AddTransient(typeof(CookingIngridientsFacade), typeof(BaseCookingIngridientsFacade));
                });

                var controller = childContainer.Resolve<BaseCookingIngridientsFacade>();
                loadingTasks.Add(controller.Initialize(token));

                _dynamicControllers.Add(controller);

                _ingridientsContainers.Push(childContainer);
            }

            return UniTask.WhenAll(loadingTasks);
        }

        private void DisposeCookingButtons()
        {
            _cookingRecepiesFacade.Dispose();

            while (_ingridientsContainers.Count != 0)
            {
                var container = _ingridientsContainers.Pop();
                container.Dispose();
            }
        }

        private void OnDoneButtonClickHappen()
        {
            DonePressed?.Invoke();
        }

        private void OnRevertButtonClickHappen()
        {
            RevertPressed?.Invoke();
        }
    }
}