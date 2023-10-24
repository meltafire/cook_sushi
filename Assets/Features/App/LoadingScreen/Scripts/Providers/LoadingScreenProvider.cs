using Utils.AssetProvider;

namespace Sushi.App.LoadingScreen
{
    public class LoadingScreenProvider : AssetInstantiator<LoadingScreenView>
    {
        public LoadingScreenProvider(ILoadingScreenParentTransformProvider transformProvider) : base(transformProvider)
        {
        }

        protected override string AssetId => LoadingScreenConstants.PrefabKey;
    }
}