using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.HeroCars;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;
    private readonly IStaticDataService _staticDataService;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
      ISaveLoadService saveLoadService, IStaticDataService staticDataService)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadService = saveLoadService;
      _staticDataService = staticDataService;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew() =>
      _progressService.Progress =
        _saveLoadService.LoadProgress()
        ?? NewProgress();

    private PlayerProgress NewProgress()
    {
      CarStaticData carData = _staticDataService.ForCar(CarTypeId.MuscleCar);
      
      var progress = new PlayerProgress(initialLevel: "Desert_1");
      
      progress.HeroGarage.AllCars = Enum.GetNames(typeof(CarTypeId)).Length;
      progress.HeroGarage.UnlockedCars = 1;
      progress.HeroGarage.ActiveCar = carData.TypeId;
      
      progress.HeroStats.Speed = carData.MaxSpeed;
      progress.HeroStats.Steering = carData.SteeringPower;
      progress.HeroStats.Acceleration = carData.Acceleration;
      
      progress.HeroWallet.Money = 0;

      return progress;
    }
  }
}