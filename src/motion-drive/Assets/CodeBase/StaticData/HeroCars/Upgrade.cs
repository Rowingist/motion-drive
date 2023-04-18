using System;

namespace CodeBase.StaticData.HeroCars
{
  [Serializable]
  public class Upgrade
  {
    public string Name;
    public int Cost;
    public int Value;

    public Upgrade(string name, int cost, int value)
    {
      Name = name;
      Cost = cost;
      Value = value;
    }
  }
}