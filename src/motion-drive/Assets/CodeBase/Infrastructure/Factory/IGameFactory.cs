using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    Joystick InputJoystick { get; }
    Task<GameObject> CreateHud();
    void Cleanup();
    Task<GameObject> CreateJoystick(Transform under);
    Task<GameObject> CreateHeroFollowingTarget(Vector3 at);
    Task<GameObject> CreateHeroCar(Vector3 at, GameObject followingTarget, GameObject checkPointsHub, IInputService inputService);
    Task WarmUp();
    Task<GameObject> CreateCheckPoint(Vector3 at, Vector3 raycastAt);
    Task<GameObject> CreateCheckpointsHub(List<GameObject> checkPoints, Vector3 initialPointPosition);
  }
}