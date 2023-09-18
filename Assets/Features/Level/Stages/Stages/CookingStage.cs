using Cysharp.Threading.Tasks;
using System.Threading;

public class CookingStage : IStage
{
    private readonly ICookingStageEvents _cookingStageEvents;
    private readonly ILevelMenuExternalEvents _levelMenuEvents;

    private UniTaskCompletionSource _completionSource;

    private LevelStages _result;

    public CookingStage(
        ICookingStageEvents cookingStageEvents,
        ILevelMenuExternalEvents levelMenuEvents)
    {
        _cookingStageEvents = cookingStageEvents;
        _levelMenuEvents = levelMenuEvents;
    }

    public async UniTask<LevelStages> Run(CancellationToken token)
    {
        _completionSource = new UniTaskCompletionSource();
        _levelMenuEvents.ButtonClicked += OnMenuButtonClicked;
        _cookingStageEvents.BackButtonClicked += OnBackButtonClicked;

        _cookingStageEvents.RequestShow(true);

        await _completionSource.Task;

        _levelMenuEvents.ButtonClicked -= OnMenuButtonClicked;
        _cookingStageEvents.BackButtonClicked -= OnBackButtonClicked;

        return _result;
    }

    private void OnBackButtonClicked()
    {
        _result = LevelStages.Idle;

        _cookingStageEvents.RequestShow(false);

        _completionSource.TrySetResult();
    }

    private void OnMenuButtonClicked()
    {
        _result = LevelStages.Quit;

        _completionSource.TrySetResult();
    }
}
