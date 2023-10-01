using System.Collections.Generic;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.Logic.MovementSettingsChangePoint
{
  public class MovementSettingsPointsHub : MonoBehaviour
  {
    private List<GameObject> _movementSettingPoints;
    private HeroFollowingTarget _heroFollowingTarget;

    public void Construct(List<GameObject> movementSettingPoints,
      HeroFollowingTarget heroFollowingTarget)
    {
      _movementSettingPoints = movementSettingPoints;
      _heroFollowingTarget = heroFollowingTarget;
      
      BindPoints();
    }

    private void BindPoints()
    {
      foreach (GameObject settingPoint in _movementSettingPoints)
      {
        settingPoint.GetComponent<MovementSettingsPoint>().Construct(_heroFollowingTarget);
        settingPoint.transform.parent = transform;
      }
    }
  }
}