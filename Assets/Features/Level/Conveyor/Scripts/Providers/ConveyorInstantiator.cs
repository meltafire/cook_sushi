using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using Utils.AssetProvider;

namespace Sushi.Level.Conveyor
{
    public class ConveyorInstantiator : AssetInstantiator<ConveyorView>
    {
        public ConveyorInstantiator(ISceneRenderReference transformProvider) : base(transformProvider)
        {
        }

        protected override string AssetId => ConveyorConstants.ConveyorPrefabName;
    }
}