using Cysharp.Threading.Tasks;
using Utils.AddressablesLoader;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenProvider : AssetInstantiator<LoadingScreenView>
    {
        private readonly ILoadingScreenParentTransformProvider _transform;

        public LoadingScreenProvider(ILoadingScreenParentTransformProvider transform)
        {
            _transform = transform;
        }

        public override UniTask<LoadingScreenView> Load()
        {
            return Instantiate(LoadingScreenConstants.PrefabKey, _transform.ParentTransform);
        }
    }
}