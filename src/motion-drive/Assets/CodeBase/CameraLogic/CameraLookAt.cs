using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraLookAt : MonoBehaviour
  {
    public float RotationDuration = 1f;

    private Coroutine _currentRotatingForces;
    
    public void OnSetNewOffset(Vector3 offset)
    {
      if (_currentRotatingForces != null)
      {
        StopCoroutine(_currentRotatingForces);
        _currentRotatingForces = null;
      }
      
      _currentRotatingForces = 
        StartCoroutine(RoutineUtils.MoveToTargetRotation(transform, Quaternion.Euler(offset), RotationDuration));
    }
  }
}