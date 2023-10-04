using CodeBase.FollowingTarget;
using CodeBase.StaticData.Level;
using UnityEngine;

namespace CodeBase.Logic.MovementSettingsChangePoint
{
  public class MovementSettingsPoint : MonoBehaviour
  {
    [SerializeField] private TriggerObserver _triggerObserver;
    
    private float _maxSpeed;
    private float _acceleration;
    private float _horizontalDragPerVelocity;

    private float _maxLiftingAngle;
    private float _maxGroundHoldingSpeed;
    private float _holdingOnGroundHeight;
    private float _snapToGroundSpeed;
    private float _groundDetectionDistance;

    private PlayerFollowingTarget _followingTarget; 
    
    public void Construct(LevelMovementSettingPointStaticData pointStaticData)
    {
      _maxSpeed = pointStaticData.MaxSpeed;
      _acceleration = pointStaticData.Acceleration;
      _horizontalDragPerVelocity = pointStaticData.HorizontalDragPerVelocity;

      _maxLiftingAngle = pointStaticData.MaxLiftingAngle;
      _maxGroundHoldingSpeed = pointStaticData.MaxGroundHoldingSpeed;
      _holdingOnGroundHeight = pointStaticData.HoldingOnGroundHeight;
      _snapToGroundSpeed = pointStaticData.SnapToGroundSpeed;
      _groundDetectionDistance = pointStaticData.GroundDetectionDistance;
    }

    public void Construct(PlayerFollowingTarget followingTarget)
    {
      _followingTarget = followingTarget;
    }
  }
}