using System;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelCheckPointsStaticData
  {
    public Vector3 Position;
    public Vector3 RaycastOnGroundOffset;

    public LevelCheckPointsStaticData(Vector3 pointPosition, Vector3 raycastOffset)
    {
      Position = pointPosition;
      RaycastOnGroundOffset = raycastOffset;
    }
  }
}