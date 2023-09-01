using Reflex.Core;

namespace Utils.Controllers.ReflexIntegration
{
    public class Factory<T> : IFactory<T> where T : Controller
    {
        private readonly Container _container;

        public Factory(Container container)
        {
            _container = container;
        }

        public T Create()
        {
            return _container.Resolve<T>();
        }
    }
}