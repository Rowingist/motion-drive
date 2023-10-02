using System;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyCarFollowingTarget : MonoBehaviour
  {
    public Rigidbody Rigidbody;
    public Transform target;
    public float SnapYOffset = 0.85f;

    private void FixedUpdate()
    {
      if (!target)
        return;

      if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 1000))
      {
        if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground") << 1)
          print(true);
          
        Vector3 newPosition = new Vector3(target.position.x, hitInfo.point.y + SnapYOffset, target.position.z);
        Rigidbody.MovePosition(newPosition);
      }
    }
  }
}