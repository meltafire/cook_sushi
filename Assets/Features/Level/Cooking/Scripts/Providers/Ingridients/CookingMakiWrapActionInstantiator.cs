using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingMakiWrapActionInstantiator : AssetInstantiator
    {
        public UniTask<ButtonView> Load(Transform transform)
        {
            return InstantiateInternal<ButtonView>(CookingConstantData.CookingMakiWrapPrefabKey, transform);
        }
    }
}