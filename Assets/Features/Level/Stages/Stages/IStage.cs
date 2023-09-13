using Cysharp.Threading.Tasks;
using System.Threading;

public interface IStage
{
    public UniTask<LevelStages> Run(CancellationToken token);
}
