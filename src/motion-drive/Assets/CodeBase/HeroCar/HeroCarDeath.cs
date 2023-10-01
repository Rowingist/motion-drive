using System;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarDeath : MonoBehaviour
  {
    public HeroCarCrashChecker CrashChecker;
    public ParticleSystem DeathFX;
    
    private HeroFollowingTarget _followingTarget;

    public void Construct(HeroFollowingTarget followingTarget)
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