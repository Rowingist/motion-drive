using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.HeroCar;
using CodeBase.HeroFollowingTarget;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Logic.CarParts;
using CodeBase.Logic.CheckPoint;
using CodeBase.Services.HeroCar;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IHeroCarProviderService _heroCarProviderService;
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    public Joystick InputJoystick { get; private set; }

    public GameFactory(IAssetProvider assets, IHeroCarProviderService heroCarProviderService)
    {
      _assets = assets;
      _heroCarProviderService = heroCarProviderService;
    }
    
    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.CheckPoint);
      await _assets.Load<GameObject>(AssetAddress.CheckPointsHub);
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);

      return hud;
    }

    public async Task<GameObject> CreateJoystick(Transform under)
    {
      GameObject joystick = await InstantiateRegisteredAsync(AssetAddress.JoystickPath, under);

      InputJoystick = joystick.GetComponentInChildren<DrivingJoystick>();

      return joystick;
    }

    public async Task<GameObject> CreateCheckPoint(Vector3 at, Vector3 raycastAt)
    {
      GameObject checkPoint = await InstantiateRegisteredAsync(AssetAddress.CheckPoint);
      checkPoint.transform.position = at;
      checkPoint.GetComponent<CheckPoint>().RaycasterToGround.transform.position = raycastAt;
      return checkPoint;
    }

    public async Task<GameObject> CreateCheckpointsHub(List<GameObject> checkPoints, Vector3 initialPointPosition)
    {
      GameObject hub = await InstantiateRegisteredAsync(AssetAddress.CheckPointsHub);
      hub.GetComponent<CheckPointsHub>().Construct(checkPoints, initialPointPosition);

      return hub;
    }

    public async Task<GameObject> CreateHeroFollowingTarget(Vector3 at, IInputService inputService)
    {
      GameObject heroFollowingTarget = await InstantiateRegisteredAsync(AssetAddress.HeroFollowingTargetPath, at);
      heroFollowingTarget.GetComponent<HeroFollowingTarget.HeroFollowingTarget>()
        .Construct(inputService);

      return heroFollowingTarget;
    }

    public async Task<GameObject> CreateHeroCar(Vector3 at, GameObject followingTarget, GameObject checkPointsHub, IInputService inputService, LoadingCurtain loadingCurtain, GameObject bodyPrefab)
    {
      CheckPointsHub pointsHub = checkPointsHub.GetComponent<CheckPointsHub>();
      
      GameObject heroCar = await InstantiateRegisteredAsync(AssetAddress.HeroCarPath);
      heroCar.GetComponent<HeroCarMove>().Construct(followingTarget.GetComponent<Rigidbody>());
      heroCar.GetComponent<HeroCarRespawn>().Construct(pointsHub, loadingCurtain);
      HeroCarRotationInAir heroCarRotationInAir = heroCar.GetComponent<HeroCarRotationInAir>();
      heroCarRotationInAir.Construct(inputService);
      heroCar.GetComponentInChildren<HeroCarRotateLookPoint>().Construct(heroCarRotationInAir, InputJoystick);
      heroCar.GetComponent<WheelsDrive>().Construct(inputService, followingTarget.GetComponent<Rigidbody>());
      heroCar.GetComponent<WheelSteering>().Construct(inputService);
      heroCar.GetComponent<BodyViewChanger>().Construct(bodyPrefab);
      
      followingTarget.GetComponent<HeroFollowingTargetRespawn>().Construct(heroCar.GetComponent<HeroCarCrashChecker>(),
        pointsHub);
      
      _heroCarProviderService.HeroCar = heroCar;

      return heroCar;
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
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }
  }
}