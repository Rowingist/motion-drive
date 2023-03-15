namespace CodeBase.Infrastructure.States
{
  public interface IState : IExitaleState
  {
    void Enter();
  }

  public interface IPayloadedState<TPayload> : IExitaleState
  {
    void Enter(TPayload sceneName);
  }

  public interface IExitaleState
  {
    void Exit();
  }
}