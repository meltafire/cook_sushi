using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils.AddressablesLoader
{
    public class AssetInstantiator : IAssetUnloader
    {
        private GameObject _cachedObject;

        public void Unload()
        {
            if (_cachedObject == null)
            {
                return;
            }

            _cachedObject.SetActive(false);
            Addressables.ReleaseInstance(_cachedObject);
            _cachedObject = null;
        }

        protected async UniTask<T> InstantiateInternal<T>(string assetId, Transform parentTransform)
        {
            var handle = Addressables.InstantiateAsync(assetId, parentTransform);
            _cachedObject = await handle.Task;

            if (_cachedObject.TryGetComponent(out T componentInstance) == false)
            {
                throw new System.NullReferenceException();
            }

            return componentInstance;
        }

        protected async UniTask<T> InstantiateInternal<T>(string assetId)
        {
            var handle = Addressables.InstantiateAsync(assetId);
            _cachedObject = await handle.Task;

            if (_cachedObject.TryGetComponent(out T componentInstance) == false)
            {
                throw new System.NullReferenceException();
            }

            return componentInstance;
        }
    }
}