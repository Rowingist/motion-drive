using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class SuspensionJointReset : MonoBehaviour
  {
    public ConfigurableJoint BodyRotation;
    public ConfigurableJoint BodySpring;

    public void StopMove()
    {
      StopBodyRotation();
      StopBodySpring();
    }

    private void StopBodyRotation()
    {
      BodyRotation.yMotion = ConfigurableJointMotion.Locked;
      BodyRotation.angularXMotion = ConfigurableJointMotion.Locked;
      BodyRotation.angularZMotion = ConfigurableJointMotion.Locked;
    }

    private void StopBodySpring() => 
      BodySpring.yMotion = ConfigurableJointMotion.Locked;


    public void Reset()
    {
      ResetBodyRotation();
      ResetBodySpring();
    }

    private void ResetBodyRotation()
    {
      BodyRotation.yMotion = ConfigurableJointMotion.Free;
      BodyRotation.angularXMotion = ConfigurableJointMotion.Free;
      BodyRotation.angularZMotion = ConfigurableJointMotion.Free;
    }

    private void ResetBodySpring() => 
      BodySpring.yMotion = ConfigurableJointMotion.Free;
  }
}