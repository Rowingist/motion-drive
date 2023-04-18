using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.HeroCars
{
  [CreateAssetMenu(fileName = "CarUpgradeProgress", menuName = "Static Data/CarUpgrade")]
  public class CarUpgradeProgress : ScriptableObject
  {
    public float AccelerationProgression;
    public float SteeringPowerProgression;
    public float NitroCapacityProgression;
    
    public List<Upgrade> AccelerationUpgrades;
    public List<Upgrade> SteeringPowerUpgrades;
    public List<Upgrade> NitroCapacityUpgrades;
    
    [ContextMenu("Set acceleration progression")]
    private void SetAccelerationLevels()
    {
      SetLevelValues(AccelerationUpgrades, AccelerationProgression);
    }

    [ContextMenu("Set steering power progression")]
    private void SetHandingLevels()
    {
      SetLevelValues(SteeringPowerUpgrades, SteeringPowerProgression );
    }

    [ContextMenu("Set nitro progression")]
    private void SetNitroLevels()
    {
      SetLevelValues(NitroCapacityUpgrades, NitroCapacityProgression);
    }

    private void SetLevelValues(List<Upgrade> upgrades, float progression)
    {
      for (int i = 0; i < upgrades.Count; i++)
      {
        var firstLevelPrice = (int)(upgrades[0].Cost + upgrades[0].Cost * progression * i);
        upgrades[i] = new Upgrade("Level_" + (i + 1), firstLevelPrice, (i + 1) * 3);
      }
    }
  }
}