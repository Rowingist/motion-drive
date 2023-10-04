using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.EnemySplines
{
  [CreateAssetMenu(menuName = "Static Data/Splines static data", fileName = "LevelSplinesStaticData")]
  public class LevelEnemySplinesStaticData : ScriptableObject
  {
    public List<SplineConfig> Configs;
  }
}