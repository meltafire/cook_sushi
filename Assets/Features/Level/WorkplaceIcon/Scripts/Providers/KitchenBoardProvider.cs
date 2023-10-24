using Utils.AssetProvider;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardProvider : AssetInstantiator<KitchenBoardView>
    {
        public KitchenBoardProvider(ISceneRenderReference transformProvider) : base(transformProvider)
        {
        }

        protected override string AssetId => KitchenBoardData.KitchenBoardPrefabKey;
    }
}