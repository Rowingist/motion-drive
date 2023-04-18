using CodeBase.StaticData;
using CodeBase.StaticData.HeroCars;
using CodeBase.StaticData.Level;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    LevelStaticData ForLevel(string sceneKey);
    CarStaticData ForCar(CarTypeId typeId);
  }
}