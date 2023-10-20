using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayNigiriInstantiator : AssetInstantiator<CookingDisplayRecepieView>
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;

        public CookingDisplayNigiriInstantiator(IIngridientsDispalyParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public override UniTask<CookingDisplayRecepieView> Load()
        {
            return Instantiate(
                CookingConstantData.CookingDisplayNigiriPrefabKey,
                _parentTransformProvider.IngridientsDispalyParentTransform);
        }
    }
}