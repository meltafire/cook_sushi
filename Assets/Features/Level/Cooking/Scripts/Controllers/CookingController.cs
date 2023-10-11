using Assets.Features.GameData.Scripts.Data;
using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler;
using Assets.Features.Level.Cooking.Scripts.States;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils.Controllers;
using Utils.Controllers.ReflexIntegration;

namespace Sushi.Level.Cooking
{
    public abstract class BaseCookingController : ResourcefulController
    {
    }

    public class CookingController : BaseCookingController,
        IRecepieParentTransformProvider,
        IIngridientsParentTransformProvider,
        IIngridientsDispalyParentTransformProvider,
        ICookingControllerGeneralButtonsProvider,
        ICookingControllerRecepieToggleProvider,
        ICookingControllerIngridentsToggleProvider
    {
        private static readonly string IngridientContainerName = "Ingridient";

        private readonly Container _container;
        private readonly ICookingControllerEvents _events;
        private readonly CookingView _view;
        private readonly CookingUiView _uiView;

        private readonly Dictionary<ControllerStatesType, ICookingControllerState> _statesDisctionary;
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly ILevelIngridientTypeProvider _levelIngridientTypeProvider;
        private readonly IFactory<CookingMakiRecepieController> _makiRecepieControllerFactory;
        private readonly IFactory<CookingNigiriRecepieController> _nigiriRecepieControllerFactory;
        private readonly RecepieDisplayHandler _recepieDisplayHandler;
        private readonly List<ResourcefulController> _dynamicControllers = new List<ResourcefulController>();

        private Stack<Container> _ingridientsContainers = new Stack<Container>();
        private CancellationToken _token;

        public event Action RevertPressed;
        public event Action DonePressed;

        public RectTransform IngridientsDispalyParentTransform => _uiView.IngridientsDispalyParentTransform;

        RectTransform IRecepieParentTransformProvider.Transform => _uiView.RecepieParentTransform;
        RectTransform IIngridientsParentTransformProvider.Transform => _uiView.IngridientsParentTransform;

        public CookingController(
            Container container,
            ICookingControllerEvents events,
            CookingView view,
            CookingUiView uiView,
            ILevelDishesTypeProvider levelDishesTypeProvider,
            ILevelIngridientTypeProvider levelIngridientTypeProvider,
            IFactory<CookingMakiRecepieController> makiRecepieControllerFactory,
            IFactory<CookingNigiriRecepieController> nigiriRecepieControllerFactory,
            RecepieDisplayHandler recepieDisplayHandler)
        {
            _container = container;
            _events = events;
            _view = view;
            _uiView = uiView;
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _levelIngridientTypeProvider = levelIngridientTypeProvider;
            _makiRecepieControllerFactory = makiRecepieControllerFactory;
            _nigiriRecepieControllerFactory = nigiriRecepieControllerFactory;
            _recepieDisplayHandler = recepieDisplayHandler;

            _statesDisctionary = new Dictionary<ControllerStatesType, ICookingControllerState>(4);
        }

        public override UniTask Initialzie(CancellationToken token)
        {
            _statesDisctionary.Add(ControllerStatesType.RecepieSelectionState, _container.Resolve<RecepieSelectionState>());
            _statesDisctionary.Add(ControllerStatesType.IngridientsState, _container.Resolve<IngridientsState>());
            _statesDisctionary.Add(ControllerStatesType.MakiIngridientsState, _container.Resolve<MakiIngridientsState>());
            _statesDisctionary.Add(ControllerStatesType.FinalizationState, _container.Resolve<FinalizationState>());

            _token = token;

            ShowWindow(false);

            ResetView();

            Start().Forget();

            _events.ShowRequest += OnShowWindowRequest;

            _uiView.BackButtonView.OnButtonPressed += OnBackButtonClickHappen;
            _uiView.RevertButtonView.OnButtonPressed += OnRevertButtonClickHappen;
            _uiView.DoneButtonView.OnButtonPressed += OnDoneButtonClickHappen;

            return UniTask.WhenAll(_recepieDisplayHandler.Initialize(token), SpawnCookingButtons(token));
        }

        public override void Dispose()
        {
            DisposeCookingButtons();

            _events.ShowRequest -= OnShowWindowRequest;

            _uiView.BackButtonView.OnButtonPressed -= OnBackButtonClickHappen;
            _uiView.RevertButtonView.OnButtonPressed -= OnRevertButtonClickHappen;
            _uiView.DoneButtonView.OnButtonPressed -= OnDoneButtonClickHappen;

            _recepieDisplayHandler.Dispose();

            base.Dispose();
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
            _uiView.ToggleRecepies(isOn);
        }

        public void ToggleIngridientButtons(bool isOn)
        {
            _uiView.ToggleIngridients(isOn);
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
                    descriptor.RegisterController<CookingIngridientController>();
                });

                var controller = childContainer.Resolve<IFactory<CookingIngridientController>>().Create();
                loadingTasks.Add(controller.Initialzie(token));

                _dynamicControllers.Add(controller);

                _ingridientsContainers.Push(childContainer);
            }

            return UniTask.WhenAll(loadingTasks);
        }

        private UniTask SpawnRecepieButtons(CancellationToken token)
        {
            var types = _levelDishesTypeProvider.GetLevelDishTypes();

            if(types.Contains(DishType.Maki))
            {
                _dynamicControllers.Add(_makiRecepieControllerFactory.Create());
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

        private void DisposeCookingButtons()
        {
            foreach(var controller in _dynamicControllers)
            {
                controller.Dispose();
            }

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