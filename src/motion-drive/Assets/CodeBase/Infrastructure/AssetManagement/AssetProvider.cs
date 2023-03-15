using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    private readonly Dictionary<string, AsyncOperationHandle> _handles =
      new Dictionary<string, AsyncOperationHandle>();

    public void Initialize() => 
      Addressables.InitializeAsync();

    public async Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : class
    {
      if (_handles.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completeHandle))
        return completeHandle.Result as T;

      return await RunWithCacheOnComplete(
        Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);
    }

    public async Task<T> Load<T>(string address) where T : class
    {
      if (!_handles.TryGetValue(address, out _))
        _handles[address] = Addressables.LoadAssetAsync<T>(address);

      if (_handles[address].IsDone)
        return _handles[address].Result as T;

      return await _handles[address].Convert<T>().Task;
    }

    public Task<GameObject> Instantiate(string address) =>
      Addressables.InstantiateAsync(address).Task;

    public Task<GameObject> Instantiate(string address, Vector3 at) => 
      Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

    public Task<GameObject> Instantiate(string address, Transform under) =>
      Addressables.InstantiateAsync(address, under).Task;
    
    public void Cleanup()
    {
      foreach (AsyncOperationHandle handle in _handles.Values) 
        Addressables.Release(handle);
      
      _handles.Clear();
    }
    
    private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
    {
      handle.Completed += completeHandle => { _handles[cacheKey] = completeHandle; };

      return await handle.Task;
    }
  }
}