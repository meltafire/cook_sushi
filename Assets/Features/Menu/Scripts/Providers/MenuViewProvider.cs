using Cysharp.Threading.Tasks;
using Sushi.Menu.Data;
using Sushi.Menu.Views;
using Sushi.SceneReference;
using Utils.AddressablesLoader;

namespace Sushi.Menu
{
    public class MenuViewProvider : AssetInstantiator
    {
        private readonly ISceneReference _sceneReference;

        public MenuViewProvider(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        public UniTask<MenuView> Load()
        {
            return InstantiateInternal<MenuView>(MenuConstants.MainMenuPrefabName, _sceneReference.OverlayCanvasTransform);
        }
    }
}