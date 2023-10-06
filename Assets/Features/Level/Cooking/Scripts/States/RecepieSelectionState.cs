using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class RecepieSelectionState : ICookingControllerState
    {
        private readonly ICookingControllerBackButtonExternalEvents _controllerServiceMethods;
        private readonly IRecipeSelectionButtonEvents _recipeSelectionButtonEvents;
        private readonly IRecepieSchemeDrawer _drawer;

        private List<CookingAction> _actions;
        private UniTaskCompletionSource _completionSource;
        private CookingScheme _chosenScheme;

        public RecepieSelectionState(
            ICookingControllerBackButtonExternalEvents controllerServiceMethods,
            IRecipeSelectionButtonEvents recipeSelectionButtonEvents,
            IRecepieSchemeDrawer drawer)
        {
            _controllerServiceMethods = controllerServiceMethods;
            _recipeSelectionButtonEvents = recipeSelectionButtonEvents;
            _drawer = drawer;
        }

        public async UniTask<ControllerStatesType> Run(List<CookingAction> actions, CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource();

            _actions = actions;

            _controllerServiceMethods.RequestToggleBackButton(true);

            _recipeSelectionButtonEvents.ShowButtons(true);
            _recipeSelectionButtonEvents.SchemeChosen += OnSchemeChosen;

            await _completionSource.Task;

            _recipeSelectionButtonEvents.ShowButtons(false);
            _recipeSelectionButtonEvents.SchemeChosen -= OnSchemeChosen;

            return _chosenScheme == CookingScheme.Maki || _chosenScheme == CookingScheme.UraMaki 
                ? ControllerStatesType.MakiIngridientsState : ControllerStatesType.IngridientsState;
        }

        private void OnSchemeChosen(CookingScheme scheme)
        {
            _actions.Add((CookingAction)scheme);

            _drawer.ShowIngridient(scheme);

            _completionSource.TrySetResult();
        }
    }
}