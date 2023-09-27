using Reflex.Core;

namespace Assets.Features.Level.Stages.Factories
{
    public class StageFactory<T> : IStageFactory<T> where T : IStage
    {
        private readonly Container _container;

        public StageFactory(Container container)
        {
            _container = container;
        }

        public T Create()
        {
            return _container.Resolve<T>();
        }
    }
}