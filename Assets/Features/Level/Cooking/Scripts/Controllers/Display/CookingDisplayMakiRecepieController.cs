using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public class CookingDisplayMakiRecepieController : ResourcefulController
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
        private readonly CookingDisplayMakiInstantiator _cookingDisplayMakiInstantiator;
        private readonly IRecepieAccountingDrawSignals _events;

        private CookingDisplayRecepieView _view;

        public CookingDisplayMakiRecepieController(
            IIngridientsDispalyParentTransformProvider parentTransformProvider,
            CookingDisplayMakiInstantiator cookingDisplayMakiInstantiator,
            IRecepieAccountingDrawSignals events)
        {
            _parentTransformProvider = parentTransformProvider;
            _cookingDisplayMakiInstantiator = cookingDisplayMakiInstantiator;
            _events = events;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefab();

            _events.DisplayMakiRecepie += OnShowRequest;
            Debug.Log("register");
        }

        public override void Dispose()
        {
            _events.DisplayMakiRecepie -= OnShowRequest;

            Debug.Log("unregister");

            base.Dispose();
        }

        private void OnShowRequest(bool isOn)
        {
            Debug.Log("showwww");
            _view.Toggle(isOn);
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_cookingDisplayMakiInstantiator);

            _view = await _cookingDisplayMakiInstantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

            _view.Toggle(false);
        }
    }
}