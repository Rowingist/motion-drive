using System.Collections;
using CodeBase.HeroCar;
using CodeBase.Logic.CheckPoint;
using UnityEngine;

namespace CodeBase.HeroFollowingTarget
{
  public class HeroFollowingTargetRespawn : MonoBehaviour
  {
    public HeroFollowingTarget FollowingTarget;
    public Rigidbody SelfRigidbody;
    
    private HeroCarCrashChecker _crashChecker;
    private CheckPointsHub _checkPointsHub;

    public void Construct(HeroCarCrashChecker crashChecker, CheckPointsHub checkPointsHub)
    {
      _crashChecker = crashChecker;
      _checkPointsHub = checkPointsHub;
      _crashChecker.Crashed += OnRespawn;
    }
    
    private void OnRespawn() => 
      StartCoroutine(Respawning());

    private IEnumerator Respawning()
    {
      DisableMovement();
      yield return new WaitForSecondsRealtime(Constants.RespawnTime);
      TransitToRespawnPosition();
      EnableMovement();
    }

    private void TransitToRespawnPosition() => 
      transform.position = _checkPointsHub.ActiveCheckPointPosition;

    private void DisableMovement()
    {
      FollowingTarget.enabled = false;
      SelfRigidbody.isKinematic = true;
    }

    private void EnableMovement()
    {
      FollowingTarget.enabled = true;
      SelfRigidbody.isKinematic = false;
    }
  }
}