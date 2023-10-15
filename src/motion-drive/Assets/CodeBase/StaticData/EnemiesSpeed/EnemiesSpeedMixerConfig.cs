using UnityEngine;

namespace CodeBase.StaticData.EnemiesSpeed
{
  [CreateAssetMenu(fileName = "NewSpeedMixer", menuName = "Static Data/EnemiesSpeedMixer")]
  public class EnemiesSpeedMixerConfig : ScriptableObject
  {
    public EnemySpeedData[] EnemiesSpeed;
  }
}