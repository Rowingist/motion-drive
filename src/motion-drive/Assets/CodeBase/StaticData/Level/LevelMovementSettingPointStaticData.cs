using System;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelMovementSettingPointStaticData
  {
    public Vector3 Position;
    
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

    public LevelMovementSettingPointStaticData(Vector3 position, float maxSpeed, float acceleration,
      float horizontalDragPerVelocity, float maxLiftingAngle, float maxGroundHoldingSpeed, float holdingOnGroundHeight,
      float snapToGroundSpeed, float groundDetectionDistance)
    {
      Position = position;
      
      MaxSpeed = maxSpeed;
      Acceleration = acceleration;
      HorizontalDragPerVelocity = horizontalDragPerVelocity;

      MaxLiftingAngle = maxLiftingAngle;
      MaxGroundHoldingSpeed = maxGroundHoldingSpeed;
      HoldingOnGroundHeight = holdingOnGroundHeight;
      SnapToGroundSpeed = snapToGroundSpeed;
      GroundDetectionDistance = groundDetectionDistance;
    }
  }
}