using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Controllers;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class RecepieSelectionState : ICookingControllerState
    {
        private readonly ICookingControllerGeneralButtonsProvider _controllerServiceMethods;
        private readonly IRecipeSelectionExternalEvents _recipeSelectionButtonEvents;
        private readonly IRecepieAccounting _drawer;
        private readonly ICookingControllerRecepieToggleProvider _toggleProvider;

        private UniTaskCompletionSource _completionSource;

        public RecepieSelectionState(
            ICookingControllerGeneralButtonsProvider controllerServiceMethods,
            IRecipeSelectionExternalEvents recipeSelectionButtonEvents,
            IRecepieAccounting drawer,
            ICookingControllerRecepieToggleProvider toggleProvider)
        {
            _controllerServiceMethods = controllerServiceMethods;
            _recipeSelectionButtonEvents = recipeSelectionButtonEvents;
            _drawer = drawer;
            _toggleProvider = toggleProvider;
        }

        public async UniTask<ControllerStatesType> Run(CancellationToken token)
        {
            _toggleProvider.ToggleRecepieButtons(true);

            _completionSource = new UniTaskCompletionSource();

            _controllerServiceMethods.ToggleBackButton(true);

            _controllerServiceMethods.ToggleDone(false);
            _controllerServiceMethods.ToggleRevert(false);
            _recipeSelectionButtonEvents.SchemeChosen += OnSchemeChosen;

            await _completionSource.Task;

            _recipeSelectionButtonEvents.SchemeChosen -= OnSchemeChosen;

            _toggleProvider.ToggleRecepieButtons(false);

            return ControllerStatesType.IngridientsState;
        }

        private void OnSchemeChosen(DishType scheme)
        {
            var cookingAction = (CookingAction)scheme;

            _drawer.ShowIngridient(cookingAction);

            _completionSource.TrySetResult();
        }
    }
}