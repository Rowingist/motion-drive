using UnityEngine;

namespace CodeBase.Services.Input
{
  public interface IInputService : IService
  {
    Vector2 Axis { get; }
    bool IsFingerUpScreen();
    bool IsFingerDownScreen();
    bool IsFingerHoldOnScreen();
    void Deactivate();
  }
}