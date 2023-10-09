using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class MakiIngridientsState : ICookingControllerState
    {
        private readonly ICookingControllerRecepieButtonsExternalEvents _buttonEvents;
        private readonly IRecepieSchemeDrawer _drawer;

        private UniTaskCompletionSource<ControllerStatesType> _completionSource;

        public MakiIngridientsState(
            ICookingControllerRecepieButtonsExternalEvents buttonEvents,
            IRecepieSchemeDrawer drawer)
        {
            _buttonEvents = buttonEvents;
            _drawer = drawer;
        }

        public async UniTask<ControllerStatesType> Run(Stack<CookingAction> actions, CancellationToken token)
        {
            _completionSource = new UniTaskCompletionSource<ControllerStatesType>();

            _buttonEvents.ToggleRevert(true);
            _buttonEvents.ToggleDone(true);
            _buttonEvents.RevertPressed += OnRevertPressed;
            _buttonEvents.DonePressed += OnDonePressed;

            var result = await _completionSource.Task;

            if(result == ControllerStatesType.RecepieSelectionState)
            {
                actions.Pop();

                _drawer.RevertIngridient();
            }

            _buttonEvents.RevertPressed -= OnRevertPressed;
            _buttonEvents.DonePressed -= OnDonePressed;

            _buttonEvents.ToggleRevert(false);
            _buttonEvents.ToggleDone(false);

            return result;
        }

        private void OnDonePressed()
        {
            _completionSource.TrySetResult(ControllerStatesType.FinalizationState);
        }

        private void OnRevertPressed()
        {
            _completionSource.TrySetResult(ControllerStatesType.RecepieSelectionState);
        }
    }
}