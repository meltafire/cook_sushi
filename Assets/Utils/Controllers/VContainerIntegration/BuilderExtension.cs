using VContainer;

namespace Utils.Controllers.VContainerIntegration
{
    public static class BuilderExtension
    {
        public static void RegisterController<T>(this IContainerBuilder builder) where T : Controller
        {
            builder.Register<T>(Lifetime.Transient);
            builder.Register<IFactory<T>, Factory<T>>(Lifetime.Singleton);
        }
    }
}
