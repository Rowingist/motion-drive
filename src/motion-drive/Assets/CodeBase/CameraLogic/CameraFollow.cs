using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraFollow : MonoBehaviour
  {
    public Rigidbody SelfRigidbody;
    public Vector3 Offset;

    private Vector3 _defaultLocalPosition;
    private Quaternion _defaultLocalRotation;

    private Transform _following;
    
    private void Awake() => 
      CacheDefaultLocalTransform();

    private void FixedUpdate()
    {
      if(_following)
        SelfRigidbody.MovePosition(_following.position + Offset);
    }

    public void SetDefaultTransform()
    {
      transform.localPosition = _defaultLocalPosition;
      transform.localRotation = _defaultLocalRotation;
    }
    
    private void CacheDefaultLocalTransform()
    {
      _defaultLocalPosition = transform.localPosition;
      _defaultLocalRotation = transform.localRotation;
    }

    public void Follow(GameObject following) => 
      _following = following.transform;
  }
}