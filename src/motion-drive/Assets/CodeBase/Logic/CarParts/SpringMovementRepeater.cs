using System;
using CodeBase.HeroCar;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class SpringMovementRepeater : MonoBehaviour
  {
    public Transform Spring;
    public float SmoothTime;
    public float MaxSpeed;

    private Vector3 _following;
    private Vector3 _velocity;
    private HeroCarOnGroundChecker _groundChecker;
    private bool IsFollowing;

    public void Construct(HeroCarOnGroundChecker onGroundChecker)
    {
      _groundChecker = onGroundChecker;
      SubscribeOnGroundCheckEvents();
    }

    private void SubscribeOnGroundCheckEvents()
    {
      _groundChecker.TookOff += StopFollowDelayed;
      _groundChecker.LandedOnGround += StartFollow;
    }

    private void StartFollow() => 
      IsFollowing = true;

    private void StopFollowDelayed()
    {
      Invoke(nameof(StopFollow), Constants.DisableJointsAfterTookOffDelay);
    }
    
    private void StopFollow() => 
      IsFollowing = false;
    
    private void Update()
    {
      if(!IsFollowing)
        return;
      
      if (_groundChecker && _groundChecker.IsOnGround)
      {
        _following.y = InverseSpringTransformPoint();
        transform.localPosition = SmoothFollowing();
      }
    }

    private void OnDestroy() => 
      CleanUp();

    private void CleanUp()
    {
      _groundChecker.TookOff -= StopFollowDelayed;
      _groundChecker.LandedOnGround -= StartFollow;
    }

    private float InverseSpringTransformPoint() =>
      transform.InverseTransformPoint(Spring.position).y;

    private Vector3 SmoothFollowing() =>
      Vector3.SmoothDamp(transform.localPosition, _following, ref _velocity, SmoothTime, MaxSpeed);
  }
}