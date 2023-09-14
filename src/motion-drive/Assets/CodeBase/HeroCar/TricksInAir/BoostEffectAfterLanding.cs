using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  public class BoostEffectAfterLanding : MonoBehaviour
  {
    public HeroCarAirTricksCounter[] TricksCounters;
    public HeroCarOnGroundChecker GroundChecker;
    public HeroCarCrashChecker CrashChecker;
    public HeroCarLandingEvaluator LandingEvaluator;
    public float BoostDuration;
    
    private bool _isBoosting;

    private Coroutine _boosting;

    public event Action<int> Boosted;
    public event Action Finished;
    
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
      
      if (_isBoosting) 
        StopActiveCoroutine();

      _boosting = StartCoroutine(Boosting());
      Boosted?.Invoke(TricksCount());
    }

    private int TricksCount() => 
      TricksCounters.Sum(counter => counter.CompletedFlipsCached);


    private void StopActiveCoroutine()
    {
      if (_boosting != null)
      {
        StopCoroutine(_boosting);
        _boosting = null;
        _isBoosting = false;
      }
    }

    private IEnumerator Boosting()
    {
      _isBoosting = true;

      float t = 0;
      while (t < 1f)
      {
        t += Time.deltaTime / BoostDuration;
        yield return null;
      }

      _isBoosting = false;
      Finished?.Invoke();
    }
  }
}