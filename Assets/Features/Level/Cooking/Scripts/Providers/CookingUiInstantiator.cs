using Sushi.SceneReference;
using Utils.AssetProvider;

namespace Sushi.Level.Cooking
{
    public class CookingUiInstantiator : AssetInstantiator<CookingUiView>
    {
        public CookingUiInstantiator(ISceneOverlayCanvasReference sceneReference) : base(sceneReference)
        {
        }

        protected override string AssetId => CookingConstantData.CookingUiPrefabKey;
    }
}