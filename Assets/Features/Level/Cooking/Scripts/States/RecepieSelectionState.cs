using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events;
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

        private List<CookingAction> _actions;
        private UniTaskCompletionSource<DishType> _completionSource;

        public RecepieSelectionState(
            ICookingControllerBackButtonExternalEvents controllerServiceMethods,
            IRecipeSelectionExternalEvents recipeSelectionButtonEvents,
            IRecepieSchemeDrawer drawer,
            CookingRecepieUiView view)
        {
            _controllerServiceMethods = controllerServiceMethods;
            _recipeSelectionButtonEvents = recipeSelectionButtonEvents;
            _drawer = drawer;
            _view = view;
        }

        public async UniTask<ControllerStatesType> Run(List<CookingAction> actions, CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource<DishType>();

            _actions = actions;

            _controllerServiceMethods.RequestToggleBackButton(true);

            _view.ShowButtons(true);
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

            _actions.Add(cookingAction);

            _drawer.ShowIngridient(cookingAction);

            _completionSource.TrySetResult(scheme);
        }
    }
}