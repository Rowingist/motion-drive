using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.HeroCar;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _services = services;

      RegisterServices();
    }

    public void Enter()
    {
      _sceneLoader.Load(Constants.InitialLevel, onLoaded: EnterLoadLevel);
    }

    public void Exit()
    {
    }

    private void RegisterServices()
    {
      RegisterStaticDataService();
      RegisterAssetProvider();

      _services.RegisterSingle<IHeroCarProviderService>(new HeroCarProviderService());
      _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(),
        _services.Single<IHeroCarProviderService>()));
      _services.RegisterSingle<IInputService>(InputService(_services.Single<GameFactory>()));
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
      _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(),
        _services.Single<IGameFactory>()));
    }

    private void RegisterStaticDataService()
    {
      IStaticDataService staticData = new StaticDataService();
      staticData.Load();
      _services.RegisterSingle(staticData);
    }

    private void RegisterAssetProvider()
    {
      IAssetProvider assetProvider = new AssetProvider();
      _services.RegisterSingle(assetProvider);
      assetProvider.Initialize();
    }

    private void EnterLoadLevel() =>
      _stateMachine.Enter<LoadProgressState>();

    private static IInputService InputService(IGameFactory gameFactory) =>
      Application.isEditor
        ? (IInputService)new StandaloneInputService(gameFactory)
        : new MobileInputService(gameFactory);
  }
}