using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraContainerFollow : MonoBehaviour
  {
    public CameraFollow CameraFollow;
    public Rigidbody SelfRigidbody;

    private void FixedUpdate()
    {
      if (CameraFollow.Following)
      {
        SelfRigidbody.MovePosition(CameraFollow.Following.position);
      }
    }
  }
}