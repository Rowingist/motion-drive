using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PlayerProgress
  {
    public WorldData WorldData;
    public Stats HeroStats;
    public Wallet HeroWallet;
    public Garage HeroGarage;

    public PlayerProgress(string initialLevel)
    {
      WorldData = new WorldData(initialLevel);
      HeroStats = new Stats();
      HeroWallet = new Wallet();
      HeroGarage = new Garage();
    }
  }
}