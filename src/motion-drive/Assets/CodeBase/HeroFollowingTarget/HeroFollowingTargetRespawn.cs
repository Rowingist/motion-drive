using CodeBase.HeroCar;
using CodeBase.Logic.CheckPoint;
using UnityEngine;

namespace CodeBase.HeroFollowingTarget
{
  public class HeroFollowingTargetRespawn : MonoBehaviour
  {
    public HeroFollowingTarget FollowingTarget;
    
    private HeroCarCrashChecker _crashChecker;
    private CheckPointsHub _checkPointsHub;

    public void Construct(HeroCarCrashChecker crashChecker, CheckPointsHub checkPointsHub)
    {
      _crashChecker = crashChecker;
      _checkPointsHub = checkPointsHub;
      _crashChecker.Crashed += OnRespawn;
    }
    
    private void OnRespawn()
    {
      FollowingTarget.enabled = false;
      transform.position = _checkPointsHub.ActiveCheckPointPosition;
      FollowingTarget.enabled = true;
    }
  }
}