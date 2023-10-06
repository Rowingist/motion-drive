using System;
using CodeBase.EnemyCar;
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

    private PlayerFollowingTargetHandler _followingTargetHandler;

    private float _enemySpeedBySpline; 
      
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

      _enemySpeedBySpline = pointStaticData.EnemySpeedBySpline;
    }

    public void Construct(PlayerFollowingTargetHandler followingTargetHandler)
    {
      _followingTargetHandler = followingTargetHandler;
    }

    private void Start()
    {
      _triggerObserver.TriggerExit += SetStageSettings;
    }

    private void OnDestroy()
    {
      _triggerObserver.TriggerExit -= SetStageSettings;
    }

    private void SetStageSettings(Collider obj)
    {
      if (obj.TryGetComponent(out PlayerFollowingTargetHandler handler))
      {
        if (handler == _followingTargetHandler)
        {
          _followingTargetHandler.CurrentSpeed = _maxSpeed;
          _followingTargetHandler.CurrentAcceleration = _acceleration;
        }
      }

      if (obj.TryGetComponent(out EnemyFollowingTarget enemyFollowingTarget))
      {
        enemyFollowingTarget.ChangeSpeed(_enemySpeedBySpline);
      }
    }
  }
}