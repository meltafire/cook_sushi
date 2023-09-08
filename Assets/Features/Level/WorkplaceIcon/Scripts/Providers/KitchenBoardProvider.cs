using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.Level.WorkplaceIcon
{
    public class KitchenBoardProvider : AssetInstantiator
    {
        public UniTask<KitchenBoardView> Load()
        {
            return InstantiateInternal<KitchenBoardView>(KitchenBoardData.KitchenBoardPrefabKey);
        }
    }
}