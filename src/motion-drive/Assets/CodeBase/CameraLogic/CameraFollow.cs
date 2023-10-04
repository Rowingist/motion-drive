using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraFollow : MonoBehaviour
  {
    public Vector3 Offset;
    public float TransitionDuration = 1;
    public float CloseDistance;

    public Transform Following { get; private set; }

    private bool _isChangingOffset;
    private Vector3 _currentVelocity = Vector3.zero;
    private Vector3 _newOffset = Vector3.zero;

    private void Update()
    {
      if (!_isChangingOffset) return;
      
      Offset = Vector3.SmoothDamp(Offset, _newOffset, ref _currentVelocity, TransitionDuration);
        
      transform.localPosition = Offset;
        
      if (Vector3.Distance(Offset, _newOffset) < CloseDistance)
        _isChangingOffset = false;
    }
    
    public void Follow(GameObject following) => 
      Following = following.transform;

    public void OnSetNewOffset(Vector3 offset)
    {
      _newOffset = offset;
      _isChangingOffset = true;
    }
    
  }
}