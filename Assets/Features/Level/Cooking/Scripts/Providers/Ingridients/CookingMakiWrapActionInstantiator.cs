using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingMakiWrapActionInstantiator : AssetInstantiator<ButtonView>
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;

        public CookingMakiWrapActionInstantiator(IIngridientsDispalyParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public override UniTask<ButtonView> Load()
        {
            return Instantiate(CookingConstantData.CookingMakiWrapPrefabKey, _parentTransformProvider.IngridientsDispalyParentTransform);
        }
    }
}