using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils.AddressablesLoader
{
    public abstract class AssetInstantiator<T>
    {
        private GameObject _cachedObject;

        public abstract UniTask<T> Load();

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

        protected async UniTask<T> Instantiate(string assetId, Transform parentTransform)
        {
            var handle = Addressables.InstantiateAsync(assetId, parentTransform);
            _cachedObject = await handle.Task;

            if (_cachedObject.TryGetComponent(out T componentInstance) == false)
            {
                throw new System.NullReferenceException();
            }

            return componentInstance;
        }
    }
}