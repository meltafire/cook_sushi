﻿using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public class CookingDisplayMakiRecepieController : ResourcefulController
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
        private readonly CookingDisplayMakiStartInstantiator _cookingDisplayMakiStartInstantiator;
        private readonly CookingDisplayMakiEndInstantiator _cookingDisplayMakiEndInstantiator;

        private CookingDisplayRecepieView _startView;
        private CookingDisplayRecepieView _endView;

        public CookingDisplayMakiRecepieController(
            IIngridientsDispalyParentTransformProvider parentTransformProvider,
            CookingDisplayMakiStartInstantiator cookingDisplayMakiStartInstantiator,
            CookingDisplayMakiEndInstantiator cookingDisplayMakiEndInstantiator)
        {
            _parentTransformProvider = parentTransformProvider;
            _cookingDisplayMakiStartInstantiator = cookingDisplayMakiStartInstantiator;
            _cookingDisplayMakiEndInstantiator = cookingDisplayMakiEndInstantiator;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefab();
        }

        public void Show(bool isOn)
        {
            _startView.Toggle(isOn);
            _endView.Toggle(isOn);
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_cookingDisplayMakiStartInstantiator);
            AttachResource(_cookingDisplayMakiEndInstantiator);

            (_startView, _endView) = await UniTask.WhenAll(
                _cookingDisplayMakiStartInstantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform),
                _cookingDisplayMakiEndInstantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform));

            _endView.transform.SetAsLastSibling();

            Show(false);
        }
    }
}