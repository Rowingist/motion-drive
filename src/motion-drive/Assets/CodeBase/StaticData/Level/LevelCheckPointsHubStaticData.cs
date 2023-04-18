using System;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelCheckPointsHubStaticData
  {
    public LevelCheckPointsStaticData[] Points;

    public LevelCheckPointsHubStaticData(LevelCheckPointsStaticData[] points)
    {
      Points = points;
    }
  }
}