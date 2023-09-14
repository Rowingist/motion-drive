using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public static class RoutineUtils
  {
    public static IEnumerator MoveToTargetRotation(Transform obj, Quaternion target, float duration = 1f)
    {
      Vector3 currentVelocity = Vector3.zero;
      
      while (obj.rotation.eulerAngles != target.eulerAngles)
      {
        Vector3 newEuler =
          Vector3.SmoothDamp(obj.rotation.eulerAngles, target.eulerAngles, ref currentVelocity, duration);

        obj.rotation = Quaternion.Euler(newEuler);
        
        yield return null;
      }
    }

    public static IEnumerator MoveToTarget(Transform obj, Vector3 target, float duration = 1)
    {
      while (obj.position != target)
      {
        obj.position = Vector3.MoveTowards(obj.position, target, Time.deltaTime * duration);
        yield return null;
      }
    }
  }
}