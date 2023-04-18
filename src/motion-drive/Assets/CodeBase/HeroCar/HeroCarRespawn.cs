using System;
using System.Collections;
using CodeBase.Logic;
using CodeBase.Logic.CheckPoint;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarRespawn : MonoBehaviour
  {
    public HeroCarCrashChecker CrashChecker;
    private CheckPointsHub _checkPointsHub;
    private LoadingCurtain _loadingCurtain;

    public event Action Completed;
    
    public void Construct(CheckPointsHub checkPointsHub, LoadingCurtain loadingCurtain)
    {
      _checkPointsHub = checkPointsHub;
      _loadingCurtain = loadingCurtain;
    }

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
      _loadingCurtain.Show();
      Completed?.Invoke();
      yield return new WaitForSecondsRealtime(Constants.TransitionToCheckpointDuration);
      _loadingCurtain.Hide();
    }
  }
}