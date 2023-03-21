using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class LevelCheckPointsStaticData
  {
    public Vector3 PointPosition;

    public LevelCheckPointsStaticData(Vector3 pointPosition)
    {
      PointPosition = pointPosition;
    }
  }
}