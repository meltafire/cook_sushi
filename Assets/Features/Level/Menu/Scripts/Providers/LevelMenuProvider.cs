using Cysharp.Threading.Tasks;
using Sushi.Level.Menu.Data;
using Sushi.SceneReference;
using Utils.AddressablesLoader;

namespace Sushi.Level.Menu
{
    public class LevelMenuProvider : AssetInstantiator
    {
        private readonly ISceneReference _sceneReference;

        public LevelMenuProvider(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        public UniTask<LevelMenuView> Load()
        {
            return InstantiateInternal<LevelMenuView>(LevelMenuConstants.LevelMenuPrefabName, _sceneReference.OverlayCanvasTransform);
        }
    }
}