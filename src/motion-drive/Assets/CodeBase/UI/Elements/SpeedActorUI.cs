using System.Collections;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class SpeedActorUI : MonoBehaviour
  {
    public HeroCarSpeedBar SpeedBar;
    public float UpdateRate;

    private PlayerFollowingTarget _followingTarget;

    public void Construct(PlayerFollowingTarget followingTarget)
    {
      _followingTarget = followingTarget;
      StartCoroutine(UpdatePlayerPositionInfo());
    }


    private IEnumerator UpdatePlayerPositionInfo()
    {
      WaitForSecondsRealtime updateRoutine = new WaitForSecondsRealtime(UpdateRate);
      while (true)
      {
        SpeedBar.SetValue(_followingTarget.Velocity, _followingTarget.MaxSpeed);

        yield return updateRoutine;
      }
    }
  }
}