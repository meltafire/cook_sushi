using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingIngridientAssetProvider : AssetInstantiator
    {
        public UniTask<IngridientButtonView> Load(Transform transform)
        {
            return InstantiateInternal<IngridientButtonView>(CookingConstantData.CookingIngredientPrefabKey, transform);
        }
    }
}