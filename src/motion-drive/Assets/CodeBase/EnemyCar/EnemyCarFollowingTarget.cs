using System;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyCarFollowingTarget : MonoBehaviour
  {
    public Rigidbody Rigidbody;
    public Transform target;

    private void FixedUpdate()
    {
      if (!target)
        return;
      
      Rigidbody.MovePosition(target.position);
    }
  }
}