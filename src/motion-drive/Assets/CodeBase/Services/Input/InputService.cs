using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public abstract class InputService : IInputService
  {
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    
    private readonly IGameFactory _gameFactory;
    
    public abstract Vector2 Axis { get; }

    public abstract bool IsFingerUpScreen();
    public abstract bool IsFingerDownScreen();
    public abstract bool IsFingerHoldOnScreen();
    public void Deactivate() => 
      _gameFactory.InputJoystick.gameObject.SetActive(false);

    protected InputService(IGameFactory gameFactory)
    {
      _gameFactory = gameFactory;
    }
    
    protected Vector2 JoystickInputAxis()
    {
      if (_gameFactory.InputJoystick == null)
        return Vector2.zero;
      
      return new Vector2(_gameFactory.InputJoystick.Horizontal, _gameFactory.InputJoystick.Vertical);
    }
  }
}