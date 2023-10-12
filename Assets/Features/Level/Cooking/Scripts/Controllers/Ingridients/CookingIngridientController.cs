using Assets.Features.Level.Cooking.Scripts.Data;
using Assets.Features.Level.Cooking.Scripts.Handler.Infrastructure;
using Assets.Features.Level.Cooking.Scripts.Providers.Ingridients;
using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Ingridients
{
    public class CookingIngridientController : ResourcefulController
    {
        private readonly CookingIngridientControllerData _data;
        private readonly CookingIngridientAssetProvider _assetProvider;
        private readonly IIngridientsParentTransformProvider _transformProvider;
        private readonly IRecepieAccounting _drawer;

        private IngridientButtonView _view;

        public CookingIngridientController(
            CookingIngridientControllerData data,
            CookingIngridientAssetProvider assetProvider,
            IIngridientsParentTransformProvider transformProvider,
            IRecepieAccounting drawer)
        {
            _data = data;
            _assetProvider = assetProvider;
            _transformProvider = transformProvider;
            _drawer = drawer;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefab();

            _view.ButtonPressed += OnButtonPressed;
        }

        public override void Dispose()
        {
            _view.ButtonPressed -= OnButtonPressed;

            base.Dispose();
        }

        private async UniTask LoadPrefab()
        {
            _view = await _assetProvider.Load(_transformProvider.Transform);

            AttachResource(_assetProvider);

            _view.SetText(_data.CookingIngridientType.ToString());
        }

        private void OnButtonPressed()
        {
            _drawer.ShowIngridient(_data.CookingIngridientType, _data.Count);
        }
    }
}