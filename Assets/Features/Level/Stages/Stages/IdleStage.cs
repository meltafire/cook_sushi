using Cysharp.Threading.Tasks;
using System.Threading;

public class IdleStage : IStage
{
    private readonly IIdleStageEvents _idleStageEvents;
    private readonly ILevelMenuExternalEvents _levelMenuEvents;
    private readonly IKitchenBoardIconExternalEvents _kitchenBoardIconExternalEvents;

    private UniTaskCompletionSource _completionSource;
    private LevelStages _result;

    public IdleStage(
        IIdleStageEvents idleStageEvents,
        ILevelMenuExternalEvents levelMenuEvents,
        IKitchenBoardIconExternalEvents kitchenBoardIconExternalEvents)
    {
        _idleStageEvents = idleStageEvents;
        _levelMenuEvents = levelMenuEvents;
        _kitchenBoardIconExternalEvents = kitchenBoardIconExternalEvents;
    }

    public async UniTask<LevelStages> Run(CancellationToken token)
    {
        _completionSource = new UniTaskCompletionSource();

        _idleStageEvents.LaunchGameplay();

        _levelMenuEvents.ButtonClicked += OnMenuButtonClicked;
        _kitchenBoardIconExternalEvents.OnClick += OnCookingButtonClicked;

        await _completionSource.Task;

        _levelMenuEvents.ButtonClicked -= OnMenuButtonClicked;
        _kitchenBoardIconExternalEvents.OnClick -= OnCookingButtonClicked;

        return _result;
    }

    private void OnMenuButtonClicked()
    {
        _result = LevelStages.Quit;

        _completionSource.TrySetResult();
    }

    private void OnCookingButtonClicked()
    {
        _result = LevelStages.Cooking;

        _completionSource.TrySetResult();
    }
}
