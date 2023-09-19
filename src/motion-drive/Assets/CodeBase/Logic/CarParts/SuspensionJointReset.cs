using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class SuspensionJointReset : MonoBehaviour
  {
    public ConfigurableJoint[] BodyRotation;
    public ConfigurableJoint BodySpring;

    public void StopMove()
    {
      StopBodyRotation();
      StopBodySpring();
    }

    private void StopBodyRotation()
    {
      foreach (ConfigurableJoint joint in BodyRotation)
      {
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
      }
    }

    private void StopBodySpring()
    {
      BodySpring.yMotion = ConfigurableJointMotion.Locked;
      Rigidbody rigid = BodySpring.GetComponent<Rigidbody>();
        
      rigid.velocity = Vector3.zero;
      rigid.angularVelocity = Vector3.zero;
    }


    public void Reset()
    {
      ResetBodyRotation();
      ResetBodySpring();
    }

    private void ResetBodyRotation()
    {
      foreach (ConfigurableJoint joint in BodyRotation)
      {
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;

        Rigidbody rigid = joint.GetComponent<Rigidbody>();
        
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
      }
    }

    private void ResetBodySpring() =>
      BodySpring.yMotion = ConfigurableJointMotion.Free;
  }
}