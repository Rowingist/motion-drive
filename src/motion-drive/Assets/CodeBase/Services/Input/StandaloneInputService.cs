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

    public override bool IsFingerUpScreen() => 
      UnityEngine.Input.GetMouseButtonUp(0);

    public override bool IsFingerDownScreen() => 
      UnityEngine.Input.GetMouseButtonDown(0);

    public override bool IsFingerHoldOnScreen() => 
      UnityEngine.Input.GetMouseButton(0);

    private static Vector2 UnityAxis() => 
      new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));

    public StandaloneInputService(IGameFactory gameFactory) : base(gameFactory)
    {
    }
  }
}