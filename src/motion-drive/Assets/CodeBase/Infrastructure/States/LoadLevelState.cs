using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.CameraLogic.Effects;
using CodeBase.Data;
using CodeBase.HeroCar;
using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.CameraSwitchPoint;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.StaticData.Level;
using CodeBase.UI;
using CodeBase.UI.Animations;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IPersistentProgressService _progressService;
    private readonly IGameFactory _gameFactory;
    private readonly IStaticDataService _staticData;
    private readonly IInputService _inputService;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
      IPersistentProgressService progressService, IGameFactory gameFactory, IStaticDataService staticData,
      IInputService inputService, IUIFactory uiFactory)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _progressService = progressService;
      _gameFactory = gameFactory;
      _staticData = staticData;
      _inputService = inputService;
      _uiFactory = uiFactory;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
      _loadingCurtain.Hide();
    }

    private async void OnLoaded()
    {
      await InitUIRoot();
      await InitGameWorld();

      InformProgressReaders();

      _gameStateMachine.Enter<GameLoopState>();
    }

    private async Task InitUIRoot() =>
      await _uiFactory.CreateUIRoot();

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      List<GameObject> checkPoints = await InitCheckPoints(levelData);
      GameObject checkPointsHub = await InitCheckPointsHub(checkPoints, levelData);

      List<GameObject> CameraSwitchPoints = await InitCameraSwitchPoints(levelData);
      await InitCameraSwitchPointsHub(CameraSwitchPoints);

      GameObject heroFollowingTarget = await InitPlayerFollowingTarget(levelData);
      GameObject heroCar = await InitHeroCar(levelData, heroFollowingTarget, checkPointsHub, _inputService);
      CameraFollow(heroCar);

      List<GameObject> movementSettingsPoints = await InitMovementSettingsPoints(levelData);
      await InitMovementSettingsPointsHub(movementSettingsPoints, heroFollowingTarget);

      InitEnemies(levelData);

      GameObject hud = await InitHud(levelData, heroCar, heroFollowingTarget);

      await InitJoystick(hud.transform);
    }

    private async Task InitEnemies(LevelStaticData levelData)
    {
      for (int i = 0; i < levelData.SplinesStaticData.Configs.Count; i++)
      {
        Vector3 at = levelData.SplinesStaticData.Configs[i].InitialPosition.AsUnityVector();
        
        GameObject spline = await InitEnemySpline(at, i);
        GameObject splineWalker = await InitEnemySplineWalker(at, spline);
        GameObject enemyFollowingTarget = await InitEnemyFollowingTarget(at, splineWalker);
        await InitEnemyCar(at, enemyFollowingTarget);
      }
    }

    private async Task InitMovementSettingsPointsHub(List<GameObject> movementSettingsPoints,
      GameObject heroFollowingTarget) =>
      await _gameFactory.CreateMoveSettingsPointsHub(movementSettingsPoints, heroFollowingTarget);

    private async Task<List<GameObject>> InitMovementSettingsPoints(LevelStaticData levelData)
    {
      var movementSettingsPoints = new List<GameObject>();

      foreach (var pointStaticData in levelData.LevelMovementSettingPointsHub.Points)
      {
        var data = pointStaticData;
        movementSettingsPoints.Add(
          await _gameFactory.CreateMoveSettingsPoint(pointStaticData.Position, data));
      }

      return movementSettingsPoints;
    }

    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private async Task<GameObject> InitHud(LevelStaticData levelData, GameObject heroCar, GameObject followingTarget)
    {
      GameObject hud = await _gameFactory.CreateHud(levelData.FinishZPosition, heroCar, followingTarget);

      return hud;
    }

    private async Task<GameObject> InitJoystick(Transform under)
    {
      GameObject joystick = await _gameFactory.CreateJoystick(under);
      return joystick;
    }

    private async Task<List<GameObject>> InitCheckPoints(LevelStaticData levelStaticData)
    {
      var checkPoints = new List<GameObject>();

      foreach (var pointsStaticData in levelStaticData.LevelCheckPointsHub.Points)
      {
        checkPoints.Add(await _gameFactory.CreateCheckPoint(pointsStaticData.Position,
          pointsStaticData.RaycastOnGroundOffset));
      }

      return checkPoints;
    }

    private async Task<GameObject> InitCheckPointsHub(List<GameObject> checkPoints, LevelStaticData levelData) =>
      await _gameFactory.CreateCheckPointsHub(checkPoints, levelData.InitialHeroPosition);

    private async Task<List<GameObject>> InitCameraSwitchPoints(LevelStaticData levelStaticData)
    {
      var cameraSwitchPoints = new List<GameObject>();

      foreach (var point in levelStaticData.LevelCameraSwitchPointsHub.Points)
      {
        cameraSwitchPoints.Add(
          await _gameFactory.CreateCameraSwitchPoint(point.Position, point.FollowSetting, point.LookAtSetting));
      }

      return cameraSwitchPoints;
    }

    private async Task<GameObject> InitCameraSwitchPointsHub(List<GameObject> cameraSwitchPoints) =>
      await _gameFactory.CreateCameraSwitchPointsHub(cameraSwitchPoints,
        Camera.main.GetComponentInParent<CameraFollow>(), Camera.main.GetComponentInParent<CameraLookAt>());

    private async Task<GameObject> InitPlayerFollowingTarget(LevelStaticData levelStaticData) =>
      await _gameFactory.CreatePlayerFollowingTarget(levelStaticData.InitialHeroPosition, _inputService,
        _progressService);

    private async Task<GameObject> InitHeroCar(LevelStaticData levelStaticData, GameObject heroFollowingTarget,
      GameObject checkPointsHub, IInputService inputService)
    {
      GameObject bodyPrefab = _staticData.ForCar(_progressService.Progress.HeroGarage.ActiveCar).Prefab;

      return await _gameFactory.CreatePlayerCar(levelStaticData.InitialHeroPosition, heroFollowingTarget,
        checkPointsHub,
        inputService, _loadingCurtain, bodyPrefab);
    }

    private async Task<GameObject> InitEnemySpline(Vector3 at, int index) =>
      await _gameFactory.CreateEnemySpline(at, index);

    private async Task<GameObject> InitEnemySplineWalker(Vector3 at, GameObject spline) =>
      await _gameFactory.CreateEnemySplineWalker(at, spline);

    private async Task<GameObject> InitEnemyFollowingTarget(Vector3 at, GameObject target) =>
      await _gameFactory.CreateEnemyFollowingTarget(at, target);

    private async Task<GameObject> InitEnemyCar(Vector3 at, GameObject followingTarget) =>
      await _gameFactory.CreateEnemyCar(at, followingTarget);

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void CameraFollow(GameObject HeroCar)
    {
      CameraFollow cameraFollow = Camera.main.GetComponentInParent<CameraFollow>();
      cameraFollow.Follow(HeroCar);
    }
  }
}