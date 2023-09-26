using Reflex.Core;

namespace Utils.Controllers.ReflexIntegration
{
    public static class BuilderExtension
    {
        public static void RegisterController<T>(this ContainerDescriptor descriptor) where T : IController
        {
            descriptor.AddTransient(typeof(T));
            descriptor.AddSingleton(typeof(Factory<T>), typeof(IFactory<T>));
        }
    }
}