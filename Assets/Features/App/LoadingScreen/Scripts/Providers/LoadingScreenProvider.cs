using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenProvider : AssetInstantiator
    {
        public UniTask<LoadingScreenView> Instantiate(Transform parent)
        {
            return InstantiateInternal<LoadingScreenView>(LoadingScreenConstants.PrefabKey, parent);
        }
    }
}