using Cysharp.Threading.Tasks;
using Sushi.Level.Conveyor.Data;
using Sushi.Level.Conveyor.Views;
using Utils.AddressablesLoader;

namespace Sushi.Level.Conveyor
{
    public class ConveyorProvider : AssetInstantiator
    {
        public UniTask<ConveyorView> Instantiate()
        {
            return InstantiateInternal<ConveyorView>(ConveyorConstants.ConveyorPrefabName);
        }
    }
}