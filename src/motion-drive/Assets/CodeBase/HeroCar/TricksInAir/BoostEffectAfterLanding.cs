using System;
using System.Collections;
using System.Linq;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  public class BoostEffectAfterLanding : MonoBehaviour
  {
    public HeroCarAirTricksCounter TricksCounter;
    public HeroCarOnGroundChecker GroundChecker;
    public HeroCarCrashChecker CrashChecker;
    public HeroCarLandingEvaluator LandingEvaluator;
    
    public float BoostDuration;
    public int MinFlipsToBoost;
    
    public bool IsBoosting;

    private Coroutine _boosting;

    public event Action Started;

    private HeroFollowingTarget _heroFollowingTarget;

    public void Construct(HeroFollowingTarget heroFollowingTarget) => 
      _heroFollowingTarget = heroFollowingTarget;

    private void Start()
    {
      CrashChecker.Crashed += StopBoostEffect;
      GroundChecker.LandedOnGround += BoostAttempt;
    }

    private void OnDestroy()
    {
      CrashChecker.Crashed -= StopBoostEffect;
      GroundChecker.LandedOnGround -= BoostAttempt;
    }

    private void StopBoostEffect()
    {
      StopActiveCoroutine();
    }

    private void BoostAttempt()
    {
      if (LandingEvaluator.IsHorizontalLandingWithSlowDown || LandingEvaluator.IsVerticalLandWithSlowDown) return;
      
      if (IsBoosting) 
        StopActiveCoroutine();

      if(EnoughFlipsReached())
        _boosting = StartCoroutine(Boosting());
    }

    private bool EnoughFlipsReached() => 
      TricksCounter.CompletedFlips >= MinFlipsToBoost;

    private void StopActiveCoroutine()
    {
      if (_boosting != null)
      {
        StopCoroutine(_boosting);
        _boosting = null;
        IsBoosting = false;
      }
    }

    private IEnumerator Boosting()
    {
      IsBoosting = true;
      _heroFollowingTarget.IsBoosting = true;
      Started?.Invoke();
      yield return new WaitForSecondsRealtime(BoostDuration);
      
      IsBoosting = false;
      _heroFollowingTarget.IsBoosting = false;
    }
  }
}