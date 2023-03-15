using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public class StandaloneInputService : InputService
  {
    public override Vector2 Axis 
    {
      get
      {
        Vector2 axis = JoystickInputAxis();

        if (axis == Vector2.zero)
          axis = UnityAxis();

        return axis;
      }
    }

    private static Vector2 UnityAxis() => 
      new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));

    public StandaloneInputService(IGameFactory gameFactory) : base(gameFactory)
    {
    }
  }
}