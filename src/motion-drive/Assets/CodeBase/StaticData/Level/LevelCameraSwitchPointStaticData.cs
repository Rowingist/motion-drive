using System;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelCameraSwitchPointStaticData
  {
    public Vector3 Position;

    public Vector3 FollowSetting;
    public Vector3 LookAtSetting;

    public LevelCameraSwitchPointStaticData(Vector3 position, Vector3 followSetting,
      Vector3 lookAtSetting)
    {
      Position = position;
      FollowSetting = followSetting;
      LookAtSetting = lookAtSetting;
    }
  }
}