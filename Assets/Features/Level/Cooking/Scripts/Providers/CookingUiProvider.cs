using Cysharp.Threading.Tasks;
using Sushi.SceneReference;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Sushi.Level.Cooking
{
    public class CookingUiProvider : AssetInstantiator
    {
        private readonly ISceneReference _sceneReference;

        public CookingUiProvider(ISceneReference sceneReference)
        {
            _sceneReference = sceneReference;
        }

        public UniTask<CookingUiView> Instantiate()
        {
            return InstantiateInternal<CookingUiView>(CookingConstantData.CookingUiPrefabKey, _sceneReference.OverlayCanvasTransform);
        }
    }
}