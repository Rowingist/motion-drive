using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.HeroCars;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string LevelsDataPath = "Static Data/Levels";
    private const string HeroCarsDataPath = "Static Data/HeroCars";
    private const string StaticDataWindowPath = "Static Data/UI/WindowStaticData";

    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<CarTypeId, CarStaticData> _cars;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;

    public void Load()
    {
      _levels = Resources
        .LoadAll<LevelStaticData>(LevelsDataPath)
        .ToDictionary(x => x.LevelKey, x => x);
      
      _cars = Resources
        .LoadAll<CarStaticData>(HeroCarsDataPath)
        .ToDictionary(x => x.TypeId, x => x);

      _windowConfigs = Resources
        .Load<WindowStaticData>(StaticDataWindowPath).Configs
        .ToDictionary(x => x.WindowId, x => x);
    }

    public LevelStaticData ForLevel(string sceneKey) => 
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData) 
        ? staticData 
        : null;

    public CarStaticData ForCar(CarTypeId typeId) => 
      _cars.TryGetValue(typeId, out CarStaticData staticData) 
        ? staticData 
        : null;

    public WindowConfig ForWindow(WindowId windowId) => 
      _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
        ? windowConfig
        : null;
  }
}