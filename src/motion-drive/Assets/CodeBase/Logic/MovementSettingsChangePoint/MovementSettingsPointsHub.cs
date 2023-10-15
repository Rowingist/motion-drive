using System.Collections.Generic;
using CodeBase.FollowingTarget;
using UnityEngine;

namespace CodeBase.Logic.MovementSettingsChangePoint
{
  public class MovementSettingsPointsHub : MonoBehaviour
  {
    private List<GameObject> _movementSettingPoints;
    private PlayerFollowingTargetHandler _playerFollowingTarget;

    public void Construct(List<GameObject> movementSettingPoints,
      PlayerFollowingTargetHandler playerFollowingTarget)
    {
      _movementSettingPoints = movementSettingPoints;
      _playerFollowingTarget = playerFollowingTarget;
      
      BindPoints();
    }

    private void BindPoints()
    {
      foreach (GameObject settingPoint in _movementSettingPoints)
      {
        settingPoint.GetComponent<MovementSettingsPoint>().Construct(_playerFollowingTarget);
        settingPoint.transform.parent = transform;
      }
    }
  }
}