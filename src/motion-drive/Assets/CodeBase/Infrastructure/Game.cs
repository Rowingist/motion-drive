using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.UI;
using CodeBase.UI.Animations;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public readonly GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
    {
      StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
    }
  }
}