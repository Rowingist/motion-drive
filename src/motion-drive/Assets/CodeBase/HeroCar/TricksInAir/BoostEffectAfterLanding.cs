using System;
using System.Collections;
using System.Linq;
using CodeBase.Car;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  public class BoostEffectAfterLanding : MonoBehaviour
  {
    public HeroCarAirTricksCounter TricksCounter;
    public CarOnGroundChecker GroundChecker;
    public PlayerCarCrashChecker CrashChecker;
    public PlayerCarLandingEvaluator LandingEvaluator;

    public float BoostDuration = 1f;
    public int MinFlipsToBoost;

    public bool IsBoosting;

    private Coroutine _boosting;

    public event Action Started;

    private PlayerFollowingTarget _playerFollowingTarget;

    public void Construct(PlayerFollowingTarget playerFollowingTarget) =>
      _playerFollowingTarget = playerFollowingTarget;

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

      if (EnoughFlipsReached())
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
      }

      IsBoosting = false;
    }

    private IEnumerator Boosting()
    {
      IsBoosting = true;
      _playerFollowingTarget.StartBoosting();
      Started?.Invoke();
      yield return new WaitForSecondsRealtime(BoostDuration + MinFlipsToBoost / 100);

      IsBoosting = false;
      _playerFollowingTarget.StopBoosting();
    }
  }
}