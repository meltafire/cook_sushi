using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayIngridientInstantiator : AssetInstantiator
    {
        public UniTask<CookingDisplayIngridientView> Load(Transform transform)
        {
            return InstantiateInternal<CookingDisplayIngridientView>(CookingConstantData.CookingDisplayIngredientPrefabKey, transform);
        }
    }
}