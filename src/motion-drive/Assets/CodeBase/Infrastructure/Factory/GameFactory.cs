using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssetProvider _assets;
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    public Joystick InputJoystick { get; private set; }

    public GameFactory(IAssetProvider assets)
    {
      _assets = assets;
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);

      return hud;
    }

    public async Task<GameObject> CreateJoystick(Transform under)
    {
      GameObject joystick = await InstantiateRegisteredAsync(AssetAddress.JoystickPath, under);
      
      InputJoystick = joystick.GetComponentInChildren<Joystick>();

      return joystick;
    }
    
    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      
      _assets.Cleanup();
    }

    private GameObject InstantiateRegistered(GameObject prefab)
    {
      GameObject gameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }
    
    private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Transform under)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath, under);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if(progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);
      
      ProgressReaders.Add(progressReader);
    }
  }
}