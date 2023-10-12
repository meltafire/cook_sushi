using Assets.Features.Level.Cooking.Scripts.Providers.Display;
using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using System.Threading;
using Utils.Controllers;

namespace Assets.Features.Level.Cooking.Scripts.Controllers.Display
{
    public class CookingDisplayNigiriRecepieController : ResourcefulController
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;
        private readonly CookingDisplayNigiriInstantiator _cookingDisplayNigiriInstantiator;

        private CookingDisplayRecepieView _view;

        public CookingDisplayNigiriRecepieController(
            IIngridientsDispalyParentTransformProvider parentTransformProvider,
            CookingDisplayNigiriInstantiator cookingDisplayNigiriInstantiator)
        {
            _parentTransformProvider = parentTransformProvider;
            _cookingDisplayNigiriInstantiator = cookingDisplayNigiriInstantiator;
        }

        public override async UniTask Initialzie(CancellationToken token)
        {
            await LoadPrefab();
        }

        public void Show(bool isOn)
        {
            _view.Toggle(isOn);
        }

        private async UniTask LoadPrefab()
        {
            AttachResource(_cookingDisplayNigiriInstantiator);

            _view = await _cookingDisplayNigiriInstantiator.Load(_parentTransformProvider.IngridientsDispalyParentTransform);

            Show(false);
        }
    }
}