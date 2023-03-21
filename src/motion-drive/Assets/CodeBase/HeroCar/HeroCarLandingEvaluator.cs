using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarLandingEvaluator : MonoBehaviour
  {
    public HeroCarOnGroundChecker GroundChecker;

    [Header("Forward/backward rotation")]
    public float AcceptableLandingHorizontalAngleMin = 45f;
    public float AcceptableLandingHorizontalAngleMax = 125f;

    [Header("Left/right rotation")]
    public float AcceptableLandingVerticalAngleMin = 45f;
    public float AcceptableLandingVerticalAngleMax = 125f;

    public bool IsLandedProperlyNoCrash { get; private set; }
    public bool IsLandedProperlyNoSlowDown { get; private set; }
    
    private void Start() => 
      GroundChecker.LandedOnGround += Evaluate;

    private void OnDestroy() => 
      GroundChecker.LandedOnGround -= Evaluate;


    private void Evaluate()
    {
      DetermineProperLandingToCrash();
      DetermineProperLandingToSlowingDown();
    }
    
    private void DetermineProperLandingToCrash() =>
      IsLandedProperlyNoCrash = IsProperHorizontalAngle(
        Vector3.SignedAngle(GroundChecker.LandInfo.transform.forward, transform.forward, transform.right));

    private bool IsProperHorizontalAngle(float landAngle) => 
      landAngle >= AcceptableLandingHorizontalAngleMin && landAngle <= AcceptableLandingHorizontalAngleMax;

    private void DetermineProperLandingToSlowingDown()
    {
      float landAngle = Vector3.SignedAngle(GroundChecker.LandInfo.transform.right, transform.right, transform.up);
      
      IsLandedProperlyNoSlowDown = IsProperVerticalAngle(landAngle);
    }

    private bool IsProperVerticalAngle(float landAngle)
    {
      return landAngle >= AcceptableLandingVerticalAngleMin && landAngle <= AcceptableLandingVerticalAngleMax;
    }
  }
}