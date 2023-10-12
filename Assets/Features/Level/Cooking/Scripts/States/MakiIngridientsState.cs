using Assets.Features.Level.Cooking.Scripts.Controllers;
using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Features.Level.Cooking.Scripts.States
{
    public class IngridientsState : ICookingControllerState
    {
        private readonly ICookingControllerGeneralButtonsProvider _buttonEvents;
        private readonly IRecepieAccounting _recepieAccounting;
        private readonly ICookingControllerIngridentsToggleProvider _toggleProvider;

        private UniTaskCompletionSource<ControllerStatesType> _completionSource;

        public IngridientsState(
            ICookingControllerGeneralButtonsProvider buttonEvents,
            IRecepieAccounting recepieAccounting,
            ICookingControllerIngridentsToggleProvider toggleProvider)
        {
            _buttonEvents = buttonEvents;
            _recepieAccounting = recepieAccounting;
            _toggleProvider = toggleProvider;
        }

        public async UniTask<ControllerStatesType> Run(CancellationToken token)
        {
            _toggleProvider.ToggleIngridientButtons(true);

            _completionSource = new UniTaskCompletionSource<ControllerStatesType>();

            _buttonEvents.ToggleRevert(true);
            _buttonEvents.ToggleDone(true);
            _buttonEvents.RevertPressed += OnRevertPressed;
            _buttonEvents.DonePressed += OnDonePressed;

            var result = await _completionSource.Task;

            _buttonEvents.RevertPressed -= OnRevertPressed;
            _buttonEvents.DonePressed -= OnDonePressed;

            _buttonEvents.ToggleRevert(false);
            _buttonEvents.ToggleDone(false);

            _toggleProvider.ToggleIngridientButtons(false);

            return result;
        }

        private void OnDonePressed()
        {
            _completionSource.TrySetResult(ControllerStatesType.FinalizationState);
        }

        private void OnRevertPressed()
        {
            _recepieAccounting.RevertIngridient();

            if (_recepieAccounting.IngridientsCount == 0)
            {
                _completionSource.TrySetResult(ControllerStatesType.RecepieSelectionState);
            }
        }
    }
}