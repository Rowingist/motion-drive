using UnityEngine;

namespace CodeBase.Logic.MovementSettingsChangePoint
{
  public class MovementSettingsPointMarker : MonoBehaviour
  {
    [Header("Movement on surface")]
    public float MaxSpeed;
    public float Acceleration;
    public float HorizontalDragPerVelocity;

    [Space(10f)] [Header("Max Lifting Angle")]
    public float MaxLiftingAngle;
    public float MaxGroundHoldingSpeed;
    public float HoldingOnGroundHeight;
    public float SnapToGroundSpeed;
    public float GroundDetectionDistance;
  }
}