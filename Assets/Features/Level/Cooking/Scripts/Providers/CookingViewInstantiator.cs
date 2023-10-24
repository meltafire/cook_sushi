using Utils.AssetProvider;

namespace Sushi.Level.Cooking
{
    public class CookingViewInstantiator : AssetInstantiator<CookingView>
    {
        public CookingViewInstantiator(ISceneRenderReference parentTransformProvider) : base(parentTransformProvider)
        {
        }

        protected override string AssetId => CookingConstantData.CookingPrefabKey;
    }
}