namespace Assets.Features.Level.Stages.Factories
{
    public interface IStageFactory<T> where T : IStage
    {
        public T Create();
    }
}