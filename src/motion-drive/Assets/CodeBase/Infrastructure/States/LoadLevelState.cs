using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.HeroCar;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.CameraSwitchPoint;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.StaticData.Level;
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

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
      IPersistentProgressService progressService, IGameFactory gameFactory, IStaticDataService staticData, IInputService inputService)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _progressService = progressService;
      _gameFactory = gameFactory;
      _staticData = staticData;
      _inputService = inputService;
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
      await InitGameWorld();

      InformProgressReaders();

      _gameStateMachine.Enter<GameLoopState>();
    }

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      GameObject hud = await InitHud();
      GameObject joystick = await InitJoystick(hud.transform);
      
      List<GameObject> checkPoints = await InitCheckPoints(levelData);
      GameObject checkPointsHub = await InitCheckPointsHub(checkPoints, levelData);

      List<GameObject> CameraSwitchPoints = await InitCameraSwitchPoints(levelData);
      InitCameraSwitchPointsHub(CameraSwitchPoints);

      GameObject heroFollowingTarget = await InitPlayerFollowingTarget(levelData);
      GameObject heroCar = await InitHeroCar(levelData, heroFollowingTarget, checkPointsHub, _inputService);
      CameraFollow(heroFollowingTarget);
    }

    private LevelStaticData LevelStaticData() =>
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private async Task<GameObject> InitHud()
    {
      GameObject hud = await _gameFactory.CreateHud();

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
      
      foreach (LevelCheckPointsStaticData pointsStaticData in levelStaticData.LevelCheckPointsHub.Points)
        checkPoints.Add(await _gameFactory.CreateCheckPoint(pointsStaticData.PointPosition, pointsStaticData.RaycastOnGroundOffset));

      return checkPoints;
    }

    private async Task<GameObject> InitCheckPointsHub(List<GameObject> checkPoints, LevelStaticData levelData) => 
      await _gameFactory.CreateCheckPointsHub(checkPoints, levelData.InitialHeroPosition);
    
    private async Task<List<GameObject>> InitCameraSwitchPoints(LevelStaticData levelStaticData)
    {
      var cameraSwitchPoints = new List<GameObject>();
      
      foreach (LevelCameraSwitchPointStaticData point in levelStaticData.LevelCameraSwitchPointsHub.Points)
        cameraSwitchPoints.Add(await _gameFactory.CreateCameraSwitchPoint(point.Position, point.FollowSetting, point.LookAtSetting));

      return cameraSwitchPoints;
    }

    private async Task<GameObject> InitCameraSwitchPointsHub(List<GameObject> cameraSwitchPoints) => 
      await _gameFactory.CreateCameraSwitchPointsHub(cameraSwitchPoints, Camera.main.GetComponentInParent<CameraFollow>(), Camera.main.GetComponentInParent<CameraLookAt>());

    private async Task<GameObject> InitPlayerFollowingTarget(LevelStaticData levelStaticData) =>
      await _gameFactory.CreateHeroFollowingTarget(levelStaticData.InitialHeroPosition, _inputService, _progressService);

    private async Task<GameObject> InitHeroCar(LevelStaticData levelStaticData, GameObject heroFollowingTarget, GameObject checkPointsHub, IInputService inputService)
    {
      GameObject bodyPrefab = _staticData.ForCar(_progressService.Progress.HeroGarage.ActiveCar).Prefab;
      
      return await _gameFactory.CreateHeroCar(levelStaticData.InitialHeroPosition, heroFollowingTarget, checkPointsHub,
        inputService, _loadingCurtain, bodyPrefab);
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void CameraFollow(GameObject HeroFollowingTarget)
    {
      Camera.main.GetComponentInParent<CameraFollow>().Follow(HeroFollowingTarget);
    }
  }
}