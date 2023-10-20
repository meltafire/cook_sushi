using Assets.Features.Level.Cooking.Scripts.Views.Ingridients.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Ingridients
{
    public class CookingNigiriRecepieInstantiator : AssetInstantiator<ButtonView>
    {
        private readonly IRecepieParentTransformProvider _parentTransformProvider;

        public CookingNigiriRecepieInstantiator(IRecepieParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public override UniTask<ButtonView> Load()
        {
            return Instantiate(CookingConstantData.CookingNigiriRecepieIngredientPrefabKey, _parentTransformProvider.Transform);
        }
    }
}