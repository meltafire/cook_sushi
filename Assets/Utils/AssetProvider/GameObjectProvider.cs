using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Utils.AddressablesLoader;

namespace Assets.Utils.AssetProvider
{
    public abstract class GameObjectProvider : IGameObjectProvider
    {
        protected abstract string AssetName { get; }

        private readonly AssetLoader _assetLoader;

        private GameObject _gameObject;

        public GameObject GameObject => _gameObject;

        public GameObjectProvider(AssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async UniTask Load(CancellationToken token)
        {
            _gameObject = await _assetLoader.Load(AssetName);
        }

        public void Unload()
        {
            _assetLoader.Unload();
        }
    }
}