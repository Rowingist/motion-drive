using System;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelMovementSettingPointsHub
  {
    public LevelMovementSettingPointStaticData[] Points;

    public LevelMovementSettingPointsHub(LevelMovementSettingPointStaticData[] points)
    {
      Points = points;
    }
  }
}