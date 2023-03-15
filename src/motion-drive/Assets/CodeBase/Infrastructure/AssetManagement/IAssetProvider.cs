using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
  public interface IAssetProvider : IService
  {
    void Initialize();
    Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : class;
    Task<T> Load<T>(string address) where T : class;
    Task<GameObject> Instantiate(string address);
    Task<GameObject> Instantiate(string address, Vector3 at);
    Task<GameObject> Instantiate(string address, Transform under);
    void Cleanup();
  }
}