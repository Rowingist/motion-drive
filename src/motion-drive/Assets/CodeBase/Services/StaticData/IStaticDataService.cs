using CodeBase.StaticData;
using CodeBase.StaticData.EnemySplines;
using CodeBase.StaticData.HeroCars;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    LevelStaticData ForLevel(string sceneKey);
    CarStaticData ForCar(CarTypeId typeId);
    WindowConfig ForWindow(WindowId windowId);
    SplineConfig ForSpline(int index);
  }
}