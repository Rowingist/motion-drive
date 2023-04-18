using System;
using CodeBase.StaticData.HeroCars;

namespace CodeBase.Data
{
  [Serializable]
  public class Garage
  {
    public int AllCars;
    public int UnlockedCars;
    public CarTypeId ActiveCar;
  }
}