using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using UnityEngine;
using Utils.AddressablesLoader;

public class CookingIngredientViewProvider : AssetInstantiator
{
    public UniTask<CookingIngredientView> Instantiate(Transform parent)
    {
            return InstantiateInternal<CookingIngredientView>(CookingConstantData.CookingIngredientPrefabKey, parent);
    }
}