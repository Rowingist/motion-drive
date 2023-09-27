using CodeBase.HeroCar;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class JointRotationRepeater : MonoBehaviour
  {
    public Transform Rotator;
    public float Speed;
    private HeroCarOnGroundChecker _groundChecker;

    private bool IsFollowing;

    public void Construct(HeroCarOnGroundChecker groundChecker)
    {
      _groundChecker = groundChecker;
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
    
    private void StopFollow()
    {
      IsFollowing = false;
    }

    private void Update()
    {
      if(!IsFollowing)
        return;

      transform.rotation = NewRotation();
    }

    private void OnDestroy() => 
      CleanUp();

    private void CleanUp()
    {
      _groundChecker.TookOff -= StopFollowDelayed;
      _groundChecker.LandedOnGround -= StartFollow;
    }

    private Quaternion NewRotation() => 
      Quaternion.Lerp(transform.rotation, Rotator.rotation, Time.deltaTime * Speed);
  }
}