using System;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class PlayerCarDeath : MonoBehaviour
  {
    public PlayerCarCrashChecker CrashChecker;
    public ParticleSystem DeathFX;
    
    private PlayerFollowingTarget _followingTarget;

    public void Construct(PlayerFollowingTarget followingTarget)
    {
      _followingTarget = followingTarget;

      CrashChecker.Crashed += OnDeathEffect;
    }

    private void OnDestroy() => 
      CrashChecker.Crashed += OnDeathEffect;

    private void OnDeathEffect()
    {
      _followingTarget.StopBoosting();
      DeathFX.gameObject.SetActive(true);
    }
  }
}