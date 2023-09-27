using UnityEngine;

namespace CodeBase
{
  public static class Constants
  {
    public const string InitialLevel = "Initial";
    public const float RespawnTime = 0.35f;
    public const float DisableJointsAfterTookOffDelay = 0.5f;
    public const float TransitionToCheckpointDuration = 1f;
    public const string PlayerLayer = "Player";
    
    public const ConfigurableJointMotion Locked = ConfigurableJointMotion.Locked;
    public const ConfigurableJointMotion Free = ConfigurableJointMotion.Free;
  }
}