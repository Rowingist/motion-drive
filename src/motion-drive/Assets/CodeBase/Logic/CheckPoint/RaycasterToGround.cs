using UnityEngine;

namespace CodeBase.Logic.CheckPoint
{
  public class RaycasterToGround : MonoBehaviour
  {
    private const float Radius = 0.25f;
    private const float YOffset = 0.5f;

    public LayerMask CastLayer;

    public (Vector3, Quaternion) PointOnGround(Vector3 entryPoint)
    {
      Vector3 castPosition = transform.position;
      castPosition.x = entryPoint.x;
      
      if (CastingSurface(castPosition, out var hitPoint))
        return (hitPoint.point + new Vector3(0f, YOffset, 0f), hitPoint.transform.rotation);

      return (transform.position, Quaternion.identity);
    }

    private bool CastingSurface(Vector3 castPosition, out RaycastHit hitPoint) => 
      Physics.Raycast(castPosition, -Vector3.up, out hitPoint, float.MaxValue, CastLayer);

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(transform.position, Radius);
    }
  }
}