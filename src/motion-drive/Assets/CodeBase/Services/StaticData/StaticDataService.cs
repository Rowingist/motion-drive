using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.EnemySplines;
using CodeBase.StaticData.HeroCars;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string LevelsDataPath = "Static Data/Levels";
    private const string HeroCarsDataPath = "Static Data/HeroCars";
    private const string WindowsStaticDataPath = "Static Data/UI/WindowStaticData";
    private const string SplinesDataPath = "Static Data/EnemySplines/SplinesStaticData_";
    private const string EnemySpline = "Enemy_Spline";
    
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<CarTypeId, CarStaticData> _cars;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;
    private Dictionary<string, SplineConfig> _splineConfigs;

    public void Load()
    {
      _levels = Resources
        .LoadAll<LevelStaticData>(LevelsDataPath)
        .ToDictionary(x => x.LevelKey, x => x);
      
      _cars = Resources
        .LoadAll<CarStaticData>(HeroCarsDataPath)
        .ToDictionary(x => x.TypeId, x => x);

      _windowConfigs = Resources
        .Load<WindowStaticData>(WindowsStaticDataPath).Configs
        .ToDictionary(x => x.WindowId, x => x);

      StringBuilder level = new StringBuilder();
      level.Append(SplinesDataPath);
      level.Append("Desert_1");
      _splineConfigs = Resources
        .Load<LevelEnemySplinesStaticData>(level.ToString()).Configs
        .ToDictionary(x => x.Label, x => x);
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

    public SplineConfig ForSpline(int index)
    {
      StringBuilder spline = new StringBuilder();
      spline.Append(EnemySpline);
      spline.Append(index);

      SplineConfig config = _splineConfigs.TryGetValue(spline.ToString(), out SplineConfig splineConfig)
        ? splineConfig
        : null;

      return config;
    }
  }
}