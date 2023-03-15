using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Services;
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
  }
}