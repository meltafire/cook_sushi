using Cysharp.Threading.Tasks;
using Sushi.Level.Menu.Data;
using Sushi.SceneReference;
using Utils.AddressablesLoader;

namespace Sushi.Level.Menu
{
    public class LevelMenuProvider : AssetInstantiator<LevelMenuView>
    {
        private readonly ISceneReference _sceneReference;

        public LevelMenuProvider(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        public override UniTask<LevelMenuView> Load()
        {
            return Instantiate(LevelMenuConstants.LevelMenuPrefabName, _sceneReference.OverlayCanvasTransform);
        }
    }
}