using UnityEngine;

namespace CodeBase.StaticData.HeroCars
{
  [CreateAssetMenu(fileName = "CarData", menuName = "Static Data/Car")]
  public class CarStaticData : ScriptableObject
  {
    public CarTypeId TypeId;
    public string Name;
    public bool IsLocked;
    
    public int Acceleration;
    public int SteeringPower;
    public int NitroCapacity;

    public float MaxSpeed;
    public float MaxAirRotationSpeed;

    public CarUpgradeProgress UpgradeProgress;

    public GameObject Prefab;
  }
}