using CodeBase.CameraLogic;
using UnityEngine;

namespace CodeBase.Logic.CameraSwitchPoint
{
  public class CameraSwitchPoint : MonoBehaviour
  {
    private const string Player = "Player";

    public TriggerObserver TriggerObserver;
    
    private CameraFollow _cameraFollow;
    private CameraLookAt _cameraLookAt;

    private Vector3 _followSetting;
    private Vector3 _lookAtSetting;

    private bool _isUsedAsFirstGate;
    
    public void Construct(CameraFollow cameraFollow, CameraLookAt cameraLookAt)
    {
      _cameraFollow = cameraFollow;
      _cameraLookAt = cameraLookAt;
    }

    public void Construct(Vector3 followSetting, Vector3 lookAtSetting)
    {
      _followSetting = followSetting;
      _lookAtSetting = lookAtSetting;
    }

    public void SetDefaultOnFirstGate()
    {
      if(_isUsedAsFirstGate)
        return;
        
      ChangeCameraTransform();
      _isUsedAsFirstGate = true;
    }
    
    private void Start() => 
      TriggerObserver.TriggerEnter += TriggerEnter;

    private void OnDestroy() => 
      TriggerObserver.TriggerEnter -= TriggerEnter;

    private void TriggerEnter(Collider other)
    {
      if (!other.TryGetComponent(out TriggerObserver triggerObserver)) return;

      if (triggerObserver.tag == Player) 
        ChangeCameraTransform();
    }

    private void ChangeCameraTransform()
    {
      ChangeFollowOffset();
      ChangeLookAtOffset();
    }

    private void ChangeFollowOffset() => 
      _cameraFollow.OnSetNewOffset(_followSetting);

    private void ChangeLookAtOffset() => 
      _cameraLookAt.OnSetNewOffset(_lookAtSetting);
  }
}