using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.CameraLogic.Effects;
using CodeBase.Car;
using CodeBase.FollowingTarget;
using CodeBase.HeroCar;
using CodeBase.HeroCar.Effects;
using CodeBase.HeroCar.TricksInAir;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.Events.LevelStart.Subscribers;
using CodeBase.Infrastructure.Events.Subscribers;
using CodeBase.Logic;
using CodeBase.Logic.CameraSwitchPoint;
using CodeBase.Logic.CarParts;
using CodeBase.Logic.CheckPoint;
using CodeBase.Logic.MovementSettingsChangePoint;
using CodeBase.Services.HeroCar;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Level;
using CodeBase.UI;
using CodeBase.UI.Animations;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using GG.Infrastructure.Utils.Swipe;
using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IHeroCarProviderService _heroCarProviderService;
    private readonly IWindowService _windowService;
    
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    public Joystick InputJoystick { get; private set; }

    public GameFactory(IAssetProvider assets, IHeroCarProviderService heroCarProviderService,
      IWindowService windowService)
    {
      _assets = assets;
      _heroCarProviderService = heroCarProviderService;
      _windowService = windowService;
    }
    
    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.CheckPoint);
      await _assets.Load<GameObject>(AssetAddress.CheckPointsHub);
    }

    public async Task<GameObject> CreateHud(float finishPosition, GameObject heroCar, GameObject followingTarget)
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
      
      GameObject indicators = await InstantiateRegisteredAsync(AssetAddress.RaceConditionIndicators);
      indicators.GetComponent<RectTransform>().SetParent(hud.GetComponent<RectTransform>(), false);
      indicators.GetComponent<RectTransform>().SetSiblingIndex(0);
      indicators.GetComponentInChildren<ProgressActorUI>().Construct(finishPosition, heroCar.GetComponent<CarMove>());
      
      indicators.GetComponentInChildren<SpeedActorUI>().Construct(followingTarget.GetComponent<HeroFollowingTarget>());
      
      hud.GetComponentInChildren<EnableFinishCongratulationWindowSubscriber>().Construct(_windowService);
      
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

    public async Task<GameObject> CreateCheckPointsHub(List<GameObject> checkPoints, Vector3 initialPointPosition)
    {
      GameObject hub = await InstantiateRegisteredAsync(AssetAddress.CheckPointsHub);
      hub.GetComponent<CheckPointsHub>().Construct(checkPoints, initialPointPosition);

      return hub;
    }
    
    public async Task<GameObject> CreateCameraSwitchPoint(Vector3 at, Vector3 followSetting, Vector3 lookAtSetting)
    {
      GameObject cameraSwitchPoint = await InstantiateRegisteredAsync(AssetAddress.CameraSwitchPoint);
      cameraSwitchPoint.transform.position = at;
      cameraSwitchPoint.GetComponent<CameraSwitchPoint>().Construct(followSetting, lookAtSetting);
      
      return cameraSwitchPoint;
    }
    
    public async Task<GameObject> CreateCameraSwitchPointsHub(List<GameObject> switchPoints, CameraFollow cameraFollow, CameraLookAt cameraLookAt)
    {
      GameObject hub = await InstantiateRegisteredAsync(AssetAddress.CameraSwitchPointsHub);
      hub.GetComponent<CameraSwitchPointsHub>().Construct(switchPoints, cameraFollow, cameraLookAt);

      return hub;
    }
    
    public async Task<GameObject> CreateMoveSettingsPoint(Vector3 at, LevelMovementSettingPointStaticData levelMovementSettingsPointData)
    {
      GameObject moveSettingsPoint = await InstantiateRegisteredAsync(AssetAddress.MovementSettingsPoint);
      moveSettingsPoint.transform.position = at;
      moveSettingsPoint.GetComponent<MovementSettingsPoint>().Construct(levelMovementSettingsPointData);
      
      return moveSettingsPoint;
    }
    
    public async Task<GameObject> CreateMoveSettingsPointsHub(List<GameObject> SettingsPoints, GameObject followingTarget)
    {
      GameObject hub = await InstantiateRegisteredAsync(AssetAddress.MovementSettingsPointsHub);
      hub.GetComponent<MovementSettingsPointsHub>().Construct(SettingsPoints, followingTarget.GetComponent<HeroFollowingTarget>());

      return hub;
    }

    public async Task<GameObject> CreateHeroFollowingTarget(Vector3 at, IInputService inputService, IPersistentProgressService playerProgress)
    {
      GameObject heroFollowingTarget = await InstantiateRegisteredAsync(AssetAddress.HeroFollowingTargetPath, at);
      var followingTarget = heroFollowingTarget.GetComponent<HeroFollowingTarget>();
      followingTarget.Construct(inputService);
      followingTarget.MaxSpeed = playerProgress.Progress.HeroStats.Speed;
      followingTarget.MaxAcceleration = playerProgress.Progress.HeroStats.Acceleration;

      followingTarget.GetComponent<DisableInputOfMovementCar>().Construct(inputService, followingTarget);
      
      return heroFollowingTarget;
    }

    public async Task<GameObject> CreateHeroCar(Vector3 at, GameObject followingTarget, GameObject checkPointsHub, IInputService inputService, LoadingCurtain loadingCurtain, GameObject bodyPrefab)
    {
      CheckPointsHub pointsHub = checkPointsHub.GetComponent<CheckPointsHub>();
      
      GameObject heroCar = await InstantiateRegisteredAsync(AssetAddress.HeroCarPath);
      heroCar.GetComponent<CarMove>().Construct(followingTarget.GetComponent<Rigidbody>());
      PlayerCarRespawn playerCarRespawn = heroCar.GetComponent<PlayerCarRespawn>();
      playerCarRespawn.Construct(pointsHub, loadingCurtain);
      heroCar.GetComponent<WheelsDrive>().Construct(inputService, followingTarget.GetComponent<Rigidbody>());
      heroCar.GetComponent<WheelSteering>().Construct(inputService);
      heroCar.GetComponent<BodyViewChanger>().Construct(bodyPrefab);
      CarOnGroundChecker carOnGroundChecker = heroCar.GetComponent<CarOnGroundChecker>();
      heroCar.GetComponent<PlayerCarSwipeRotationInAir>().Construct(
        carOnGroundChecker, heroCar.GetComponent<SwipeListener>(), followingTarget.GetComponent<HeroFollowingTarget>());
      heroCar.GetComponentInChildren<SpringMovementRepeater>().Construct(carOnGroundChecker);
      HeroCarAirTricksCounter heroCarAirTricksCounter = heroCar.GetComponentInChildren<HeroCarAirTricksCounter>();
      heroCar.GetComponentInChildren<LandingEffect>().Construct(carOnGroundChecker, playerCarRespawn, 
        heroCarAirTricksCounter);
      
      
      heroCar.GetComponentInChildren<GainingRotationPointsInAirEffect>().Construct( heroCar.GetComponent<PlayerCarSwipeRotationInAir>(), 
        carOnGroundChecker, heroCarAirTricksCounter);

      BoostEffectAfterLanding boostEffectAfterLanding = heroCar.GetComponent<BoostEffectAfterLanding>();
      CameraFOVEffect fovEffect = heroCar.GetComponentInChildren<CameraFOVEffect>();
      fovEffect.Construct(boostEffectAfterLanding);
      heroCar.GetComponent<BoostEffectOnStartLevelSubscriber>().Construct(fovEffect);

      JointRotationRepeater[] repeaters = heroCar.GetComponentsInChildren<JointRotationRepeater>();
      heroCar.GetComponent<PlayerCarDeath>().Construct(followingTarget.GetComponent<HeroFollowingTarget>());

      foreach (JointRotationRepeater repeater in repeaters) 
        repeater.Construct(carOnGroundChecker);

      followingTarget.GetComponent<HeroFollowingTargetRespawn>().Construct(heroCar.GetComponent<PlayerCarCrashChecker>(),
        pointsHub);
      followingTarget.GetComponent<HeroFollowingTargetHandler>()
        .Construct(boostEffectAfterLanding);
      
      boostEffectAfterLanding.Construct(followingTarget.GetComponent<HeroFollowingTarget>());
      
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