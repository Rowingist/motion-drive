using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.HeroCars;
using CodeBase.StaticData.Level;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string LevelsDataPath = "Static Data/Levels";
    private const string HeroCarsDataPath = "Static Data/HeroCars";

    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<CarTypeId, CarStaticData> _cars;

    public void Load()
    {
      _levels = Resources
        .LoadAll<LevelStaticData>(LevelsDataPath)
        .ToDictionary(x => x.LevelKey, x => x);
      
      _cars = Resources
        .LoadAll<CarStaticData>(HeroCarsDataPath)
        .ToDictionary(x => x.TypeId, x => x);
    }

    public LevelStaticData ForLevel(string sceneKey) => 
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData) 
        ? staticData 
        : null;

    public CarStaticData ForCar(CarTypeId typeId) => 
      _cars.TryGetValue(typeId, out CarStaticData staticData) 
        ? staticData 
        : null;
  }
}