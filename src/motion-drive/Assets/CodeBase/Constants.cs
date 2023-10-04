using UnityEngine;

namespace CodeBase
{
  public static class Constants
  {
    public const string InitialLevel = "Initial";
    public const float RespawnTime = 0.65f;
    public const float DisableJointsAfterTookOffDelay = 0.5f;
    public const float TransitionToCheckpointDuration = 0.8f;
    public const string PlayerLayer = "Player";
    
    public const ConfigurableJointMotion Locked = ConfigurableJointMotion.Locked;
    public const ConfigurableJointMotion Free = ConfigurableJointMotion.Free;

    public static readonly Vector3 EulerAngleX360 = new(360f, 0f, 0f);
    public static readonly Vector3 EulerAngleMinusX360 = new(-360f, 0f, 0f);
    public static readonly Vector3 EulerAngleY360 = new(0f, 360f, 0f);
    public static readonly Vector3 EulerAngleMinusY360 = new(0f, -360f, 0f);
  }
}