using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class RecepieSelectionState : ICookingControllerState
    {
        private readonly ICookingControllerBackButtonExternalEvents _controllerServiceMethods;
        private readonly IRecipeSelectionExternalEvents _recipeSelectionButtonEvents;
        private readonly IRecepieSchemeDrawer _drawer;
        private readonly CookingRecepieUiView _view;
        private readonly ICookingControllerRecepieButtonsExternalEvents _buttonEvents;

        private Stack<CookingAction> _actions;
        private UniTaskCompletionSource<DishType> _completionSource;

        public RecepieSelectionState(
            ICookingControllerBackButtonExternalEvents controllerServiceMethods,
            IRecipeSelectionExternalEvents recipeSelectionButtonEvents,
            IRecepieSchemeDrawer drawer,
            CookingRecepieUiView view,
            ICookingControllerRecepieButtonsExternalEvents buttonEvents)
        {
            _controllerServiceMethods = controllerServiceMethods;
            _recipeSelectionButtonEvents = recipeSelectionButtonEvents;
            _drawer = drawer;
            _view = view;
            _buttonEvents = buttonEvents;
        }

        public async UniTask<ControllerStatesType> Run(Stack<CookingAction> actions, CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource<DishType>();

            _actions = actions;

            _controllerServiceMethods.RequestToggleBackButton(true);

            _view.ShowButtons(true);
            _buttonEvents.ToggleDone(false);
            _buttonEvents.ToggleRevert(false);
            _recipeSelectionButtonEvents.SchemeChosen += OnSchemeChosen;

            var result = await _completionSource.Task;

            _view.ShowButtons(false);
            _recipeSelectionButtonEvents.SchemeChosen -= OnSchemeChosen;

            return result == DishType.Maki || result == DishType.UraMaki 
                ? ControllerStatesType.MakiIngridientsState : ControllerStatesType.IngridientsState;
        }

        private void OnSchemeChosen(DishType scheme)
        {
            var cookingAction = (CookingAction)scheme;

            _actions.Push(cookingAction);

            _drawer.ShowIngridient(cookingAction);

            _completionSource.TrySetResult(scheme);
        }
    }
}