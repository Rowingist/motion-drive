using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraLookAt : MonoBehaviour
  {
    public void OnSetNewOffset(Vector3 offset)
    {
      transform.rotation = Quaternion.Euler(offset);
    }
  }
}