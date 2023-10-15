using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : InputService
  {
    private readonly IGameFactory _gameFactory;
    public override Vector2 Axis => JoystickInputAxis();

    public override bool IsFingerUpScreen() => 
      _gameFactory.InputJoystick.IsPointerUp;

    public override bool IsFingerDownScreen() => 
      _gameFactory.InputJoystick.IsPointerDown;

    public override bool IsFingerHoldOnScreen()
    {
      if (_gameFactory == null)
        return false;
      
      if (_gameFactory.InputJoystick == null)
        return false;
      
      return _gameFactory.InputJoystick.IsPointer;
    }

    public MobileInputService(IGameFactory gameFactory) : base(gameFactory) => 
      _gameFactory = gameFactory;
  }
}