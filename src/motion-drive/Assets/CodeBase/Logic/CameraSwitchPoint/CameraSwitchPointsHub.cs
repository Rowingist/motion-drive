using System.Collections.Generic;
using CodeBase.CameraLogic;
using UnityEngine;

namespace CodeBase.Logic.CameraSwitchPoint
{
  public class CameraSwitchPointsHub : MonoBehaviour
  {
    private List<GameObject> _levelCameraSwitchPoints;
    private CameraFollow _cameraFollow;
    private CameraLookAt _cameraLookAt;
    
    public void Construct(List<GameObject> cameraSwitchPoints, CameraFollow cameraFollow, CameraLookAt cameraLookAt)
    {
      _levelCameraSwitchPoints = cameraSwitchPoints;
      _cameraFollow = cameraFollow;
      _cameraLookAt = cameraLookAt;
      
      BindSwitchPoints();
      SetCameraTransformAtStart();
    }

    private void BindSwitchPoints()
    {
      foreach (GameObject point in _levelCameraSwitchPoints)
        point.GetComponent<CameraSwitchPoint>().Construct(_cameraFollow, _cameraLookAt);
    }

    private void SetCameraTransformAtStart() => 
      _levelCameraSwitchPoints[0].GetComponent<CameraSwitchPoint>().SetDefaultOnFirstGate();
  }
}