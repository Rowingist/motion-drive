using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  [RequireComponent(typeof(HeroCarOnGroundChecker), typeof(HeroCarCrashChecker), typeof(HeroCarSwipeRotationInAir))]
  public class HeroCarAirTricksCounter : MonoBehaviour
  {
    private HeroCarOnGroundChecker _groundCheck;
    private HeroCarCrashChecker _crashChecker;
    private HeroCarSwipeRotationInAir _rotationInAir;
    
    public int CompletedFlips { get; private set; }

    private void Awake()
    {
      _groundCheck = GetComponent<HeroCarOnGroundChecker>();
      _crashChecker = GetComponent<HeroCarCrashChecker>();
      _rotationInAir = GetComponent<HeroCarSwipeRotationInAir>();
    }

    private void Start()
    {
      _groundCheck.LandedOnGround += ClearUp;
      _crashChecker.Crashed += ClearUp;
      _rotationInAir.Flipped += CollectFlip;
    }

    private void CollectFlip()
    {
      CompletedFlips++;
    }

    private void OnDestroy()
    {
      _groundCheck.LandedOnGround -= ClearUp;
      _crashChecker.Crashed -= ClearUp;
    }

    private void ClearUp()
    {
      Invoke(nameof(Clear), Constants.RespawnTime);
    }

    private void Clear() => 
      CompletedFlips = 0;
  }
}