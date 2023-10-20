﻿using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayMakiEndInstantiator : AssetInstantiator<CookingDisplayRecepieView>
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;

        public CookingDisplayMakiEndInstantiator(IIngridientsDispalyParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public override async UniTask<CookingDisplayRecepieView> Load()
        {
            var component = await Instantiate(
                CookingConstantData.CookingDisplayMakiEndPrefabKey,
                _parentTransformProvider.IngridientsDispalyParentTransform);

            component.gameObject.transform.SetAsLastSibling();

            return component;
        }
    }
}