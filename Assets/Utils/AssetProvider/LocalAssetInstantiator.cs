using Assets.Utils.AssetProvider;
using UnityEngine;

namespace Utils.AssetProvider
{
    public class LocalAssetInstantiator<T>
    {
        private readonly IGameObjectProvider _gameObjectProvider;
        private readonly IParentTransformProvider _transformProvider;

        private GameObject _cachedObject;

        public LocalAssetInstantiator(IGameObjectProvider gameObjectProvider, IParentTransformProvider transformProvider)
        {
            _gameObjectProvider = gameObjectProvider;
            _transformProvider = transformProvider;
        }

        public T Load()
        {
            _cachedObject = GameObject.Instantiate(
                _gameObjectProvider.GameObject,
                _transformProvider.Transform);

            _cachedObject.SetActive(false);

            return _cachedObject.GetComponent<T>();
        }

        public void Unload()
        {
            if (_cachedObject == null)
            {
                return;
            }

            _cachedObject.SetActive(false);

            GameObject.Destroy(_cachedObject);
        }
    }
}