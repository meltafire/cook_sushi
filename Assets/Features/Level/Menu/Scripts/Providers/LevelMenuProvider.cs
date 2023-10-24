using Sushi.Level.Menu.Data;
using Sushi.SceneReference;
using Utils.AssetProvider;

namespace Sushi.Level.Menu
{
    public class LevelMenuProvider : AssetInstantiator<LevelMenuView>
    {
        public LevelMenuProvider(ISceneOverlayCanvasReference sceneReference) : base(sceneReference)
        {
        }

        protected override string AssetId => LevelMenuConstants.LevelMenuPrefabName;
    }
}