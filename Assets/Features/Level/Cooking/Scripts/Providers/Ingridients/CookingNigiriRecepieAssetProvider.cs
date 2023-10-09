using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using UnityEditor;
using UnityEngine;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingNigiriRecepieAssetProvider : CookingRecepieButtonAssetInstantiator
    {
        public override UniTask<ButtonView> Load(Transform transform)
        {
            return InstantiateInternal<ButtonView>(CookingConstantData.CookingNigiriRecepieIngredientPrefabKey, transform);
        }
    }
}