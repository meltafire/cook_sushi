using Sushi.Menu.Data;
using Sushi.Menu.Views;
using Sushi.SceneReference;
using Utils.AssetProvider;

namespace Sushi.Menu
{
    public class MenuViewProvider : AssetInstantiator<MenuView>
    {
        public MenuViewProvider(ISceneOverlayCanvasReference sceneReference) : base(sceneReference)
        {
        }

        protected override string AssetId => MenuConstants.MainMenuPrefabName;
    }
}