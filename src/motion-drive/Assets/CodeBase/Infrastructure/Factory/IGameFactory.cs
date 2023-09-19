using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Logic.CameraSwitchPoint;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.HeroCars;
using CodeBase.StaticData.Level;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    Joystick InputJoystick { get; }
    Task<GameObject> CreateHud(GameObject heroCar);
    void Cleanup();
    Task<GameObject> CreateJoystick(Transform under);
    Task<GameObject> CreateHeroFollowingTarget(Vector3 at, IInputService inputService, IPersistentProgressService carData);
    Task<GameObject> CreateHeroCar(Vector3 at, GameObject followingTarget, GameObject checkPointsHub, IInputService inputService, LoadingCurtain loadingCurtain, GameObject bodyPrefab);
    Task WarmUp();
    Task<GameObject> CreateCheckPoint(Vector3 at, Vector3 raycastAt);
    Task<GameObject> CreateCheckPointsHub(List<GameObject> checkPoints, Vector3 initialPointPosition);
    Task<GameObject> CreateCameraSwitchPoint(Vector3 at, Vector3 followSetting, Vector3 lookAtSetting);
    Task<GameObject> CreateCameraSwitchPointsHub(List<GameObject> switchPoints, CameraFollow cameraFollow, CameraLookAt cameraLookAt);
    Task<GameObject> CreateMoveSettingsPoint(Vector3 at,
      LevelMovementSettingPointStaticData levelMovementSettingsPointData);
    Task<GameObject> CreateMoveSettingsPointsHub(List<GameObject> SettingsPoints,
      GameObject followingTarget);
  }
}