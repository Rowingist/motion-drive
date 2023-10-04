using CodeBase.Car;
using UnityEngine;

namespace CodeBase.HeroCar.TricksInAir
{
  [RequireComponent(typeof(CarOnGroundChecker), typeof(PlayerCarCrashChecker), typeof(PlayerCarSwipeRotationInAir))]
  public class HeroCarAirTricksCounter : MonoBehaviour
  {
    private CarOnGroundChecker _groundCheck;
    private PlayerCarCrashChecker _crashChecker;
    private PlayerCarSwipeRotationInAir _rotationInAir;
    
    public int CompletedFlips { get; private set; }

    private void Awake()
    {
      _groundCheck = GetComponent<CarOnGroundChecker>();
      _crashChecker = GetComponent<PlayerCarCrashChecker>();
      _rotationInAir = GetComponent<PlayerCarSwipeRotationInAir>();
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