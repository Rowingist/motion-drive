using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarLandingEvaluator : MonoBehaviour
  {
    public HeroCarOnGroundChecker GroundChecker;

    [Header("Forward/backward rotation")] public float AcceptableLandingHorizontalAngleMin = 45f;
    public float AcceptableLandingHorizontalAngleMax = 125f;

    [Header("Left/right rotation")] public float AcceptableLandingVerticalAngleMin = 45f;
    public float AcceptableLandingVerticalAngleMax = 125f;

    public bool IsVerticalLandWithSlowDown { get; private set; }
    public bool IsHorizontalLandingWithSlowDown { get; private set; }

    private void Start() =>
      GroundChecker.LandedOnGround += Evaluate;

    private void OnDestroy() =>
      GroundChecker.LandedOnGround -= Evaluate;

    private void Evaluate()
    {
      DetermineVerticalLandingToSlowingDown();
      DetermineHorizontalLandingToSlowingDown();
    }

    private void DetermineVerticalLandingToSlowingDown()
    {
      float landAngle =
        Vector3.SignedAngle(GroundChecker.LandInfo.transform.forward, transform.forward, transform.right);

      IsVerticalLandWithSlowDown = AngleIsOutOfProperLandRange(landAngle, AcceptableLandingVerticalAngleMin,
        AcceptableLandingVerticalAngleMax);
    }

    private void DetermineHorizontalLandingToSlowingDown()
    {
      float landAngle = Vector3.SignedAngle(GroundChecker.LandInfo.transform.right, transform.right, transform.up);

      IsHorizontalLandingWithSlowDown = AngleIsOutOfProperLandRange(landAngle, AcceptableLandingHorizontalAngleMin,
        AcceptableLandingHorizontalAngleMax);
    }

    private bool AngleIsOutOfProperLandRange(float landAngle, float minAngle, float maxAngle) =>
      landAngle >= minAngle && landAngle <= maxAngle;
  }
}