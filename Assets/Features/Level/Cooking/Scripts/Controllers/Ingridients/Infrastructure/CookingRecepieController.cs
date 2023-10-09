using Assets.Features.GameData.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Events.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public abstract class CookingRecepieController: ResourcefulController
    {
        private readonly IIngridientsParentTransformProvider _parentTransformProvider;
        private readonly CookingRecepieButtonAssetInstantiator _assetInstantiator;
        private readonly IRecipeSelectionEvents _events;

        private ButtonView _view;

        public CookingRecepieController(
            IIngridientsParentTransformProvider parentTransformProvider,
            CookingRecepieButtonAssetInstantiator assetInstantiator,
            IRecipeSelectionEvents events)
        {
            _parentTransformProvider = parentTransformProvider;
            _assetInstantiator = assetInstantiator;
            _events = events;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefab();

            _view.OnButtonPressed += OnButtonPressed;
        }

        public override void Dispose()
        {
            _view.OnButtonPressed -= OnButtonPressed;

            base.Dispose();
        }

        protected abstract DishType ReportData();

        private void OnButtonPressed()
        {
            _events.ReportSchemeChosen(ReportData());
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_assetInstantiator);

            _view = await _assetInstantiator.Load(_parentTransformProvider.IngridientsParentTransform);
        }
    }
}