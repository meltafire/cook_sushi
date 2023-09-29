using Assets.Features.GameData.Scripts.Providers;
using Assets.Features.Level.Cooking.Scripts.Controllers.Infrastructure;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Sushi.Level.Cooking;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class DishSelectionState : ICookingControllerState
    {
        private readonly ILevelDishesTypeProvider _levelDishesTypeProvider;
        private readonly CookingTypeMenuUiView _view;
        private readonly IStateChanger _stateChanger;
        private readonly Container _container;

        private UniTaskCompletionSource _taskCompletionSource;

        public DishSelectionState(
            ILevelDishesTypeProvider levelDishesTypeProvider,
            CookingTypeMenuUiView view,
            IStateChanger stateChanger,
            Container container)
        {
            _levelDishesTypeProvider = levelDishesTypeProvider;
            _view = view;
            _stateChanger = stateChanger;
            _container = container;
        }

        public async UniTask Run(CancellationToken token)
        {
            var dishTypes = _levelDishesTypeProvider.GetLevelDishTypes();

            if (dishTypes.Count == 1)
            {
                var type = dishTypes.Contains(DishType.Nigiri) ? DishType.Nigiri : DishType.Maki;

                HandleDishSelected(type);
            }
            else
            {
                await ShowMenu(dishTypes, token);
            }
        }

        private UniTask ShowMenu(HashSet<DishType> set, CancellationToken token)
        {
            _taskCompletionSource = new UniTaskCompletionSource();

            _view.ToggleNigiriButton(set.Contains(DishType.Nigiri));
            _view.ToggleMakiButton(set.Contains(DishType.Maki));

            _view.Toggle(true);

            _view.OnButtonClick += OnCookingTypeClick;

            return _taskCompletionSource.Task;
        }

        private void OnCookingTypeClick(DishType type)
        {
            _view.OnButtonClick -= OnCookingTypeClick;

            _taskCompletionSource.TrySetResult();

            _view.Toggle(false);

            HandleDishSelected(type);

            _taskCompletionSource.TrySetResult();
        }

        private void HandleDishSelected(DishType dishType)
        {
            if (dishType == DishType.Nigiri)
            {
                _stateChanger.SetState(_container.Resolve<NigiriState>());
            }
            else
            {
                _stateChanger.SetState(_container.Resolve<MakiState>());
            }
        }
    }
}