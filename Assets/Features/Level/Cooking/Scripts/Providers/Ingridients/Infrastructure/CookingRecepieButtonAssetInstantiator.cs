using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients.Infrastructure
{
    public abstract class CookingRecepieButtonAssetInstantiator : AssetInstantiator
    {
        public abstract UniTask<ButtonView> Load(Transform transform);
    }
}