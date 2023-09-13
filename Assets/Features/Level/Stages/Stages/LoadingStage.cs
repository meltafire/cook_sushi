using Cysharp.Threading.Tasks;
using System.Threading;

public class LoadingStage : IStage
{
    private readonly ILoadingStageEvents _stageEventsProvider;
    private readonly ILoadingScreenExternalEvents _loadingScreenExternalEvents;

    private int _loadingFeatureCount;
    private UniTaskCompletionSource _completionSource;

    public LoadingStage(ILoadingStageEvents stageEventsProvider, ILoadingScreenExternalEvents loadingScreenExternalEvents)
    {
        _stageEventsProvider = stageEventsProvider;
        _loadingScreenExternalEvents = loadingScreenExternalEvents;
    }

    public async UniTask<LevelStages> Run(CancellationToken token)
    {
        _completionSource = new UniTaskCompletionSource();

        _stageEventsProvider.FeatureLoaded += OnFeatureLoaded;
        _stageEventsProvider.FeatureStartedLoading += OnFeatureStartedLoading;

        _stageEventsProvider.RequesLoad();

         await _completionSource.Task;

        _stageEventsProvider.FeatureLoaded -= OnFeatureLoaded;
        _stageEventsProvider.FeatureStartedLoading -= OnFeatureStartedLoading;

        _loadingScreenExternalEvents.Show(false);

        return LevelStages.Idle;
    }

    private void OnFeatureStartedLoading()
    {
        ++_loadingFeatureCount;
    }

    private void OnFeatureLoaded()
    {
        --_loadingFeatureCount;

        if (_loadingFeatureCount == 0)
        {
            _completionSource.TrySetResult();
        }
    }
}
