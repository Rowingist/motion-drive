using System;
using System.Collections;
using CodeBase.Logic.CheckPoint;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRespawn : MonoBehaviour
  {
    public HeroCarCrashChecker CrashChecker;
    private CheckPointsHub _checkPointsHub;

    public event Action Completed;
    
    public void Construct(CheckPointsHub checkPointsHub) => 
      _checkPointsHub = checkPointsHub;

    private void Start() => 
      CrashChecker.Crashed += OnRespawn;

    private void OnDestroy() => 
      CrashChecker.Crashed += OnRespawn;

    private void OnRespawn()
    {
      StartCoroutine(Respawning());
    }

    private IEnumerator Respawning()
    {
      yield return new WaitForSecondsRealtime(Constants.RespawnTime);
      Completed?.Invoke();
    }
  }
}