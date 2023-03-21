using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class LevelCheckPointsStaticData
  {
    public Vector3 PointPosition;
    public Vector3 RaycastOnGroundOffset;

    public LevelCheckPointsStaticData(Vector3 pointPosition, Vector3 raycastOffset)
    {
      PointPosition = pointPosition;
      RaycastOnGroundOffset = raycastOffset;
    }
  }
}