using Cysharp.Threading.Tasks;
using Sushi.SceneReference;
using Utils.AddressablesLoader;

namespace Sushi.Level.Cooking
{
    public class CookingUiInstantiator : AssetInstantiator<CookingUiView>
    {
        private readonly ISceneReference _sceneReference;

        public CookingUiInstantiator(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        public override UniTask<CookingUiView> Load()
        {
            return Instantiate(CookingConstantData.CookingUiPrefabKey, _sceneReference.OverlayCanvasTransform);
        }
    }
}