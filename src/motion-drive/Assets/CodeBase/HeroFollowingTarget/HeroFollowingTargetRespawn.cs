using System.Collections;
using CodeBase.HeroCar;
using CodeBase.Logic.CheckPoint;
using DG.Tweening;
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
    }

    private void TransitToRespawnPosition() =>
      StartCoroutine(Transiting(_checkPointsHub.ActiveCheckPointPosition));

    private IEnumerator Transiting(Vector3 checkPointPosition)
    {
      float t = 0;
      
      while (t < 1)
      {
        transform.DOMove(checkPointPosition, Constants.RespawnTime).SetEase(Ease.Linear);
        t += Time.deltaTime;
        yield return null;
      }
      
      EnableMovement();
    }
    
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