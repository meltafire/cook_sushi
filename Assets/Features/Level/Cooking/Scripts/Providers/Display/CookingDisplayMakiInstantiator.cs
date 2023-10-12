﻿using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayMakiStartInstantiator : AssetInstantiator
    {
        public UniTask<CookingDisplayRecepieView> Load(Transform transform)
        {
            return InstantiateInternal<CookingDisplayRecepieView>(CookingConstantData.CookingDisplayMakiStartPrefabKey, transform);
        }
    }
}