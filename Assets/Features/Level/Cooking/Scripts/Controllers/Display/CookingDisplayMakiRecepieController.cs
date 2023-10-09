using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Display.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public class CookingDisplayMakiRecepieController : ResourcefulController
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
        private readonly CookingDisplayMakiInstantiator _cookingDisplayMakiInstantiator;
        private readonly IMakiDisplayEvents _events;

        private CookingDisplayRecepieView _view;

        public CookingDisplayMakiRecepieController(
            IIngridientsDispalyParentTransformProvider parentTransformProvider,
            CookingDisplayMakiInstantiator cookingDisplayMakiInstantiator,
            IMakiDisplayEvents events)
        {
            _parentTransformProvider = parentTransformProvider;
            _cookingDisplayMakiInstantiator = cookingDisplayMakiInstantiator;
            _events = events;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefab();

            _events.ToggleRequest += OnShowRequest;
        }

        public override void Dispose()
        {
            _events.ToggleRequest -= OnShowRequest;

            base.Dispose();
        }

        private void OnShowRequest(bool isOn)
        {
            _view.Toggle(isOn);
        }

        private string GetTextForStep(CookingStep step)
        {
            return step.ToString();
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_cookingDisplayMakiInstantiator);

            _view = await _cookingDisplayMakiInstantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

            _view.Toggle(false);
        }
    }
}