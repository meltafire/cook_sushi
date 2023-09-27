using Assets.Features.Level.Conveyor.Scripts.Events;
using Assets.Features.Level.Stages.Factories;
using Cysharp.Threading.Tasks;
using System.Threading;

public class IdleStage : IStage
{
    private readonly ILevelMenuExternalEvents _levelMenuEvents;
    private readonly IKitchenBoardIconExternalEvents _kitchenBoardIconExternalEvents;
    private readonly IStageFactory<CookingStage> _cookingStageFactory;
    private readonly IConveyorTileExternalEvents _conveyorTileExternalEvents;

    private UniTaskCompletionSource _completionSource;
    private CancellationTokenSource _menuQuitTokenSource;

    public IdleStage(
        IStageFactory<CookingStage> cookingStageFactory,
        ILevelMenuExternalEvents levelMenuEvents,
        IKitchenBoardIconExternalEvents kitchenBoardIconExternalEvents,
        IConveyorTileExternalEvents conveyorTileExternalEvents)
    {
        _cookingStageFactory = cookingStageFactory;
        _levelMenuEvents = levelMenuEvents;
        _kitchenBoardIconExternalEvents = kitchenBoardIconExternalEvents;
        _conveyorTileExternalEvents = conveyorTileExternalEvents;
    }

    public async UniTask Run(CancellationToken token)
    {
        _menuQuitTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        _completionSource = new UniTaskCompletionSource();

        LaunchGameplay();

        ToggleInteractions(true);

        await _completionSource.Task;

        ToggleInteractions(false);
    }

    private void OnMenuButtonClicked()
    {
        _menuQuitTokenSource.Cancel();

        _completionSource.TrySetResult();
    }

    private async void OnCookingButtonClicked()
    {
        _kitchenBoardIconExternalEvents.RequestButtonToggle(false);

        var cookingStage = _cookingStageFactory.Create();

        await cookingStage.Run(_menuQuitTokenSource.Token);
    }

    private void ToggleInteractions(bool isOn)
    {
        if (isOn)
        {
            _levelMenuEvents.OnClick += OnMenuButtonClicked;
            _kitchenBoardIconExternalEvents.OnClick += OnCookingButtonClicked;
        }
        else
        {
            _levelMenuEvents.OnClick -= OnMenuButtonClicked;
            _kitchenBoardIconExternalEvents.OnClick -= OnCookingButtonClicked;
        }

        _kitchenBoardIconExternalEvents.RequestButtonToggle(isOn);
        _levelMenuEvents.RequestButtonToggle(isOn);
    }

    private void LaunchGameplay()
    {
        _conveyorTileExternalEvents.RequestToggleMovement(true);
    }
}