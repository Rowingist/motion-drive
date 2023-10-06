using UnityEngine;

namespace CodeBase.Logic.MovementSettingsChangePoint
{
  [CreateAssetMenu(fileName = "StageSettingsType", menuName = "Static Data/StageSettings")]
  public class StageSettingsTypeData : ScriptableObject
  {
    public string Label;
    
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
    
    [Space(10f)] [Header("EnemySetting")]
    public float EnemySpeedBySpline;
  }
}