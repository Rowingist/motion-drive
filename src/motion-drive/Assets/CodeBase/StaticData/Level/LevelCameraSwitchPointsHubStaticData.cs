using System;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelCameraSwitchPointsHubStaticData
  {
    public LevelCameraSwitchPointStaticData[] Points;

    public LevelCameraSwitchPointsHubStaticData(LevelCameraSwitchPointStaticData[] points)
    {
      Points = points;
    }
  }
}