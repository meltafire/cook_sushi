using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.Level.Cooking
{
    public class CookingViewProvider : AssetInstantiator
    {
        public UniTask<CookingView> Load()
        {
            return InstantiateInternal<CookingView>(CookingConstantData.CookingPrefabKey);
        }
    }
}