using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : InputService
  {
    public override Vector2 Axis => JoystickInputAxis();

    public MobileInputService(IGameFactory gameFactory) : base(gameFactory)
    {
    }
  }
}