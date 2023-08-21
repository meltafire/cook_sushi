using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utils.AddressableLoader
{
    public class AssetLoader
    {
        private AsyncOperationHandle<GameObject> _handle;

        public async UniTask<GameObject> Load(string assetName)
        {
            _handle = Addressables.LoadAssetAsync<GameObject>(assetName);

            await _handle.Task;

            return HandleResult(assetName);
        }

        public void Release()
        {
            Addressables.Release(_handle);
        }

        private GameObject HandleResult(string assetName)
        {
            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                return _handle.Result;
            }
            else
            {
                Debug.LogError($"Asset for {assetName} failed to load.");

                return null;
            }
        }
    }
}