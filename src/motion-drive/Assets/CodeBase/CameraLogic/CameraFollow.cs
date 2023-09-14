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

    public void Follow(GameObject following) => 
      _following = following.transform;

    public void OnSetNewOffset(Vector3 offset) => 
      Offset = new Vector3(offset.x, offset.y, offset.z);

    private void CacheDefaultLocalTransform()
    {
      _defaultLocalPosition = transform.localPosition;
      _defaultLocalRotation = transform.localRotation;
    }
  }
}