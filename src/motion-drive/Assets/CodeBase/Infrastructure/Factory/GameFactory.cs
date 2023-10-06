using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.CameraLogic.Effects;
using CodeBase.Car;
using CodeBase.EnemyCar;
using CodeBase.FollowingTarget;
using CodeBase.HeroCar;
using CodeBase.HeroCar.Effects;
using CodeBase.HeroCar.TricksInAir;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.Events.LevelStart.Subscribers;
using CodeBase.Infrastructure.Events.Subscribers;
using CodeBase.Logic;
using CodeBase.Logic.Bezier;
using CodeBase.Logic.Bezier.Movement;
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
using UnityEngine.SceneManagement;

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

      indicators.GetComponentInChildren<SpeedActorUI>()
        .Construct(followingTarget.GetComponent<PlayerFollowingTarget>());

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

    public async Task<GameObject> CreateCameraSwitchPointsHub(List<GameObject> switchPoints, CameraFollow cameraFollow,
      CameraLookAt cameraLookAt)
    {
      GameObject hub = await InstantiateRegisteredAsync(AssetAddress.CameraSwitchPointsHub);
      hub.GetComponent<CameraSwitchPointsHub>().Construct(switchPoints, cameraFollow, cameraLookAt);

      return hub;
    }

    public async Task<GameObject> CreateMoveSettingsPoint(Vector3 at,
      LevelMovementSettingPointStaticData levelMovementSettingsPointData)
    {
      GameObject moveSettingsPoint = await InstantiateRegisteredAsync(AssetAddress.MovementSettingsPoint);
      moveSettingsPoint.transform.position = at;
      moveSettingsPoint.GetComponent<MovementSettingsPoint>().Construct(levelMovementSettingsPointData);

      return moveSettingsPoint;
    }

    public async Task<GameObject> CreateMoveSettingsPointsHub(List<GameObject> SettingsPoints,
      GameObject followingTarget)
    {
      GameObject hub = await InstantiateRegisteredAsync(AssetAddress.MovementSettingsPointsHub);
      hub.GetComponent<MovementSettingsPointsHub>()
        .Construct(SettingsPoints, followingTarget.GetComponent<PlayerFollowingTargetHandler>());

      return hub;
    }

    public async Task<GameObject> CreatePlayerFollowingTarget(Vector3 at, IInputService inputService,
      IPersistentProgressService playerProgress, float stageMaxSpeed, float stageMaxAcceleration)
    {
      GameObject heroFollowingTarget = await InstantiateRegisteredAsync(AssetAddress.PlayerFollowingTargetPath, at);

      PlayerFollowingTarget followingTarget = heroFollowingTarget.GetComponent<PlayerFollowingTarget>();
      followingTarget.Construct(inputService);
      followingTarget.MaxSpeed = playerProgress.Progress.HeroStats.Speed + stageMaxSpeed;
      followingTarget.MaxAcceleration = playerProgress.Progress.HeroStats.Acceleration + stageMaxAcceleration;

      followingTarget.GetComponent<DisableInputOfMovementCar>().Construct(inputService, followingTarget);

      return heroFollowingTarget;
    }

    public async Task<GameObject> CreatePlayerCar(Vector3 at, GameObject followingTarget, GameObject checkPointsHub,
      IInputService inputService, LoadingCurtain loadingCurtain, GameObject bodyPrefab)
    {
      GameObject heroCar = await InstantiateRegisteredAsync(AssetAddress.PlayerCarPath);

      PlayerCarRespawn playerCarRespawn = heroCar.GetComponent<PlayerCarRespawn>();
      CheckPointsHub pointsHub = checkPointsHub.GetComponent<CheckPointsHub>();
      CarOnGroundChecker carOnGroundChecker = heroCar.GetComponent<CarOnGroundChecker>();
      HeroCarAirTricksCounter heroCarAirTricksCounter = heroCar.GetComponentInChildren<HeroCarAirTricksCounter>();
      Rigidbody followingRigidbody = followingTarget.GetComponent<Rigidbody>();
      BoostEffectAfterLanding boostEffectAfterLanding = heroCar.GetComponent<BoostEffectAfterLanding>();
      CameraFOVEffect fovEffect = heroCar.GetComponentInChildren<CameraFOVEffect>();
      var repeaters = heroCar.GetComponentsInChildren<JointRotationRepeater>();

      heroCar.GetComponent<CarMove>().Construct(followingRigidbody);

      playerCarRespawn.Construct(pointsHub, loadingCurtain);

      heroCar.GetComponent<WheelsDrive>().Construct(inputService, followingRigidbody);
      heroCar.GetComponent<WheelSteering>().Construct(inputService);

      heroCar.GetComponent<BodyViewChanger>().Construct(bodyPrefab);
      heroCar.GetComponent<PlayerCarSwipeRotationInAir>().Construct(carOnGroundChecker,
        heroCar.GetComponent<SwipeListener>(), followingTarget.GetComponent<PlayerFollowingTarget>());
      heroCar.GetComponentInChildren<SpringMovementRepeater>().Construct(carOnGroundChecker);
      heroCar.GetComponentInChildren<LandingEffect>()
        .Construct(carOnGroundChecker, playerCarRespawn, heroCarAirTricksCounter);

      heroCar.GetComponentInChildren<GainingRotationPointsInAirEffect>().Construct(
        heroCar.GetComponent<PlayerCarSwipeRotationInAir>(),
        carOnGroundChecker, heroCarAirTricksCounter);

      fovEffect.Construct(boostEffectAfterLanding);
      heroCar.GetComponent<BoostEffectOnStartLevelSubscriber>().Construct(fovEffect);

      heroCar.GetComponent<PlayerCarDeath>().Construct(followingTarget.GetComponent<PlayerFollowingTarget>());

      foreach (JointRotationRepeater repeater in repeaters)
        repeater.Construct(carOnGroundChecker);

      followingTarget.GetComponent<PlayerFollowingTargetRespawn>().Construct(
        heroCar.GetComponent<PlayerCarCrashChecker>(),
        pointsHub);
      followingTarget.GetComponent<PlayerFollowingTargetHandler>()
        .Construct(boostEffectAfterLanding);

      boostEffectAfterLanding.Construct(followingTarget.GetComponent<PlayerFollowingTarget>());

      _heroCarProviderService.HeroCar = heroCar;

      return heroCar;
    }

    public async Task<GameObject> CreateEnemySpline(Vector3 at, int index)
    {
      StringBuilder splinePath = new StringBuilder();
      splinePath.Append(AssetAddress.EnemySplinePath);
      splinePath.Append(SceneManager.GetActiveScene().name);
      splinePath.Append('_');
      splinePath.Append(index);

      GameObject spline = await InstantiateRegisteredAsync(splinePath.ToString(), at);

      return spline;
    }

    public async Task<GameObject> CreateEnemySplineWalker(Vector3 at, GameObject spline, float startDuration)
    {
      GameObject splineWalker = await InstantiateRegisteredAsync(AssetAddress.EnemySplineWalkerPath, at);

      splineWalker.GetComponent<SplineWalker>().Construct(spline.GetComponent<BezierSpline>(), startDuration);
      
      return splineWalker;
    }

    public async Task<GameObject> CreateEnemyFollowingTarget(Vector3 at, GameObject target)
    {
      GameObject followingTarget = await InstantiateRegisteredAsync(AssetAddress.EnemyFollowingTargetPath, at);
      followingTarget.GetComponent<EnemyFollowingTarget>().Construct(target.transform);

      return followingTarget;
    }

    public async Task<GameObject> CreateEnemyCar(Vector3 at, GameObject followingTarget, GameObject splineWalker)
    {
      GameObject enemyCar = await InstantiateRegisteredAsync(AssetAddress.EnemyCarPath, at);

      Rigidbody followingRigidbody = followingTarget.GetComponent<Rigidbody>();
      EnemyFollowingTarget enemyFollowingTarget = followingTarget.GetComponent<EnemyFollowingTarget>();

      enemyFollowingTarget.Construct(enemyCar.GetComponent<CarOnGroundChecker>());
      enemyCar.GetComponent<CarMove>().Construct(followingRigidbody);
      enemyCar.GetComponent<EnemyCarDeath>().Construct(followingRigidbody);
      SplineWalker walker = splineWalker.GetComponent<SplineWalker>();
      enemyCar.GetComponent<EnemyRespawn>()
        .Construct(walker, enemyFollowingTarget, enemyCar.GetComponentInChildren<CarJoints>());
      enemyCar.GetComponent<EnemyRespawnPositionUpdater>().Construct(walker);

      return enemyCar;
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