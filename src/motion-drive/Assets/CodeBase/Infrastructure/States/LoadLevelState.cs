using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IPersistentProgressService _progressService;
    private readonly IGameFactory _gameFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, 
      IPersistentProgressService progressService, IGameFactory gameFactory)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _progressService = progressService;
      _gameFactory = gameFactory;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
      _loadingCurtain.Hide();
    }

    private async void OnLoaded()
    {
      await InitGameWorld();
      
      InformProgressReaders();
      
      _gameStateMachine.Enter<GameLoopState>();
    }

    private async Task InitGameWorld()
    {
      GameObject hud = await InitHud();

      await InitJoystick(hud.transform);
    }

    private async Task<GameObject> InitHud()
    {
      GameObject hud = await _gameFactory.CreateHud();

      return hud;
    }

    private async Task InitJoystick(Transform under)
    {
      GameObject joystick = await _gameFactory.CreateJoystick(under);
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }
  }
}