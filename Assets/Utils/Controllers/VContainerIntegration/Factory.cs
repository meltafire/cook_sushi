using VContainer;

namespace Utils.Controllers.VContainerIntegration
{
    public class Factory<T> : IFactory<T> where T : Controller
    {
        private readonly IObjectResolver _resolver;

        public Factory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T Create()
        {
            return _resolver.Resolve<T>();
        }
    }
}