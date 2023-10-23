using Assets.Features.Level.Cooking.Scripts.Views.Display.Infrastructure;
using Cysharp.Threading.Tasks;
using Sushi.Level.Cooking;
using Utils.AddressablesLoader;

namespace Assets.Features.Level.Cooking.Scripts.Providers.Display
{
    public class CookingDisplayIngridientInstantiator : AssetInstantiator<CookingDisplayIngridientView>
    {
        private readonly IIngridientsDispalyParentTransformProvider _parentTransformProvider;

        public CookingDisplayIngridientInstantiator(IIngridientsDispalyParentTransformProvider parentTransformProvider)
        {
            _parentTransformProvider = parentTransformProvider;
        }

        public async override UniTask<CookingDisplayIngridientView> Load()
        {
            var view = await Instantiate(
                CookingConstantData.CookingDisplayIngredientPrefabKey,
                _parentTransformProvider.IngridientsDispalyParentTransform);

            view.gameObject.SetActive(false);

            return view;
        }
    }
}