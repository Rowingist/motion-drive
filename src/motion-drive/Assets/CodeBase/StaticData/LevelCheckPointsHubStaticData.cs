using System;

namespace CodeBase.StaticData
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