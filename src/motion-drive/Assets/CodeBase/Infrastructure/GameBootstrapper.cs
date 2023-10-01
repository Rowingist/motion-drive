using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.UI;
using CodeBase.UI.Animations;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    public LoadingCurtain CurtainPrefab;
    
    private Game _game;
    
    private void Awake()
    {
      _game = new Game(this, Instantiate(CurtainPrefab));
      _game.StateMachine.Enter<BootstrapState>();
      
      DontDestroyOnLoad(this);
    }
  }
}
