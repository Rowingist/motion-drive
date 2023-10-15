using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class CrashChecker : MonoBehaviour
  {
    public Vector3 MaxCrashAngleCheck;
    public Vector3 MinCrashAngleCheck;
    public float CrashCheckInterval = 1f;
    public Transform CheckingRigidBody;

    public SuspensionJointReset JointReset;

    private void Start()
    {
      StartCoroutine(CheckingCrash());
    }

    private IEnumerator CheckingCrash()
    {
      WaitForSecondsRealtime interval = new WaitForSecondsRealtime(CrashCheckInterval);
      
      while (true)
      {
        if (CheckingRigidBody.localEulerAngles.x > MaxCrashAngleCheck.x ||
            CheckingRigidBody.localEulerAngles.y > MaxCrashAngleCheck.y ||
            CheckingRigidBody.localEulerAngles.z > MaxCrashAngleCheck.z ||
            CheckingRigidBody.localEulerAngles.x > MinCrashAngleCheck.x ||
            CheckingRigidBody.localEulerAngles.y > MinCrashAngleCheck.y ||
            CheckingRigidBody.localEulerAngles.z > MinCrashAngleCheck.z
           )
        {
          JointReset.StopMove();
          JointReset.Reset();
        }
        
        yield return interval;
      }
    }
  }
}