using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils.AssetProvider
{
    public abstract class AssetInstantiator<T>
    {
        private readonly IParentTransformProvider _transformProvider;

        protected abstract string AssetId { get; }

        private GameObject _cachedObject;

        protected AssetInstantiator(IParentTransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public async UniTask<T> Load()
        {
            var handle = Addressables.InstantiateAsync(AssetId, _transformProvider.Transform);
            _cachedObject = await handle.Task;

            _cachedObject.SetActive(false);

            if (_cachedObject.TryGetComponent(out T componentInstance) == false)
            {
                throw new System.NullReferenceException();
            }

            return componentInstance;
        }

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
    }
}