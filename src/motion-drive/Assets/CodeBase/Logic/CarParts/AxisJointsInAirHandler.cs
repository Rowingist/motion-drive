using CodeBase.HeroCar;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class AxisJointsInAirHandler : MonoBehaviour
  {
    public ConfigurableJoint[] TargetJoints;
    public HeroCarOnGroundChecker GroundChecker;
    public SuspensionJointReset SuspensionJoints;

    private SoftJointLimit[] _defaultZAngularLimits;

    private void Start()
    {
      CacheAngularLimits();

      GroundChecker.TookOff += OnStopAnyMovement;
      GroundChecker.LandedOnGround += OnStartMovement;
    }

    private void OnDestroy()
    {
      GroundChecker.TookOff -= OnStopAnyMovement;
      GroundChecker.LandedOnGround -= OnStartMovement;
    }

    private void OnStopAnyMovement()
    {
      SuspensionJoints.StopMove();
      ReduceAngularLimits();
    }

    private void OnStartMovement()
    {
      SuspensionJoints.Reset();
      ResetAngularLimits();
    }

    private void CacheAngularLimits()
    {
      _defaultZAngularLimits = new SoftJointLimit[TargetJoints.Length];
      
      for (int i = 0; i < TargetJoints.Length; i++)
        _defaultZAngularLimits[i] = TargetJoints[i].angularZLimit;
    }

    private void ReduceAngularLimits()
    {
      foreach (ConfigurableJoint targetJoint in TargetJoints)
      {
        SoftJointLimit newLimit = new SoftJointLimit();
        newLimit.limit = 0;
        targetJoint.angularZLimit = newLimit;
      }
    }
    
    private void ResetAngularLimits()
    {
      for (int i = 0; i < TargetJoints.Length; i++)
         TargetJoints[i].angularZLimit = _defaultZAngularLimits[i];
    }
  }
}