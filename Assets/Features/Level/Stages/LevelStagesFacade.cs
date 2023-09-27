using Assets.Features.Level.Stages.Factories;
using Cysharp.Threading.Tasks;
using System.Threading;

public class LevelStagesFacade
{
    private readonly IStageFactory<IdleStage> _idleStageFactory;

    private IStage _baseStage;

    public LevelStagesFacade(IStageFactory<IdleStage> idleStageFactory)
    {
        _idleStageFactory = idleStageFactory;
    }

    public UniTask Launch(CancellationToken token)
    {
        _baseStage = _idleStageFactory.Create();

        return _baseStage.Run(token);
    }
}
