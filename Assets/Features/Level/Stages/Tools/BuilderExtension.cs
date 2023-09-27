using Assets.Features.Level.Stages.Factories;
using Reflex.Core;

namespace Assets.Features.Level.Stages.Tools
{
    public static class BuilderExtension
    {
        public static void RegisterStage<T>(this ContainerDescriptor descriptor) where T : IStage
        {
            descriptor.AddTransient(typeof(T));
            descriptor.AddSingleton(typeof(StageFactory<T>), typeof(IStageFactory<T>));
        }
    }
}