using System.Collections;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraFollow : MonoBehaviour
  {
    public Rigidbody SelfRigidbody;
    public Vector3 Offset;
    public float TransitionDuration = 1;
    public float CloseDistance;

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

    public void OnSetNewOffset(Vector3 offset)
    {
      StartCoroutine(MoveToTarget(offset, TransitionDuration));
    }

    private void CacheDefaultLocalTransform()
    {
      _defaultLocalPosition = transform.localPosition;
      _defaultLocalRotation = transform.localRotation;
    }
    
    private IEnumerator MoveToTarget(Vector3 target, float duration = 1)
    {
      Vector3 currentVelocity = Vector3.zero;

      while (Vector3.Distance(Offset, target) > CloseDistance)
      {
        Offset = Vector3.SmoothDamp(Offset, target, ref currentVelocity, duration);

        yield return null;
      }
    }
  }
}