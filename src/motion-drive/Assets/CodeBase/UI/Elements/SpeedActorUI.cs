using System.Collections;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class SpeedActorUI : MonoBehaviour
  {
    public HeroCarSpeedBar SpeedBar;
    public float UpdateRate;

    private HeroFollowingTarget _followingTarget;

    public void Construct(HeroFollowingTarget followingTarget)
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