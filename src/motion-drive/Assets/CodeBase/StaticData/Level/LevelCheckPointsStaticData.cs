using System;
using CodeBase.StaticData.EnemiesSpeed;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
  [Serializable]
  public class LevelCheckPointsStaticData
  {
    public Vector3 Position;
    public Vector3 RaycastOnGroundOffset;
    public EnemiesSpeedMixerConfig EnemiesSpeedMixerConfig;
    
    public LevelCheckPointsStaticData(Vector3 pointPosition, Vector3 raycastOffset,
      EnemiesSpeedMixerConfig enemiesSpeedMixerConfig)
    {
      Position = pointPosition;
      RaycastOnGroundOffset = raycastOffset;
      EnemiesSpeedMixerConfig = enemiesSpeedMixerConfig;
    }
  }
}