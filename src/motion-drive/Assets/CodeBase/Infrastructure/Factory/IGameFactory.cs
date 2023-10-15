using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.EnemiesSpeedHandler;
using CodeBase.FollowingTarget;
using CodeBase.Logic.Bezier;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData.EnemiesSpeed;
using CodeBase.StaticData.Level;
using CodeBase.UI.Animations;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    Joystick InputJoystick { get; }
    
    Task WarmUp();
    Task<GameObject> CreateHud(float finishZPosition, GameObject heroCar, GameObject followingTarget);
    Task<GameObject> CreateJoystick(Transform under);
    Task<GameObject> CreateCheckPoint(Vector3 at, Vector3 raycastAt, EnemiesSpeedMixerConfig enemiesSpeedMixerConfig);
    Task<GameObject> CreateEnemiesSpeedMixer();
    Task<GameObject> CreateCheckPointsHub(List<GameObject> checkPoints, Vector3 initialPointPosition, GameObject enemiesSpeedMixer);
    Task<GameObject> CreateCameraSwitchPoint(Vector3 at, Vector3 followSetting, Vector3 lookAtSetting);
    Task<GameObject> CreateCameraSwitchPointsHub(List<GameObject> switchPoints, CameraFollow cameraFollow, CameraLookAt cameraLookAt);
    Task<GameObject> CreateMoveSettingsPoint(Vector3 at, LevelMovementSettingPointStaticData levelMovementSettingsPointData);
    Task<GameObject> CreateMoveSettingsPointsHub(List<GameObject> SettingsPoints, GameObject followingTarget);
    Task<GameObject> CreatePlayerFollowingTarget(Vector3 at, IInputService inputService, IPersistentProgressService carData, float stageMaxSpeed, float stageMaxAcceleration);
    Task<GameObject> CreatePlayerCar(Vector3 at, GameObject followingTarget, GameObject checkPointsHub, IInputService inputService, LoadingCurtain loadingCurtain, GameObject bodyPrefab);
    Task<GameObject> CreateEnemySpline(Vector3 ad, int index);
    Task<GameObject> CreateEnemySplineWalker(Vector3 at, GameObject spline, float startDuration, GameObject playerFollowingTarget);
    Task<GameObject> CreateEnemyFollowingTarget(Vector3 at, GameObject target);
    Task<GameObject> CreateEnemyCar(Vector3 at, GameObject followingTarget, GameObject splineWalker, IInputService inputService);

    void Cleanup();
  }
}