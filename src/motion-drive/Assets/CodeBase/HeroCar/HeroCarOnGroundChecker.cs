using System;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarOnGroundChecker : MonoBehaviour
  {
    public LayerMask RaycastLayer;
    public float TakingOffHeight;
    
    public Color HeightGizmoColor;

    public bool IsOnGround { get; private set; }
    public RaycastHit LandInfo { get; private set; }

    public event Action TookOff;

    public event Action LandedOnGround;

    private void FixedUpdate()
    {
      if (!GroundIsRaycast(out RaycastHit hitInfo))
      {
        if(!IsOnGround) return;
        
        TakeOff();
        
        return;
      }

      if (IsOnGround) return;
      
      CacheLandInfo(hitInfo);
      LandOnGround();
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = HeightGizmoColor;
      Gizmos.DrawRay(transform.position, -Vector3.up * TakingOffHeight);
    }

    private bool GroundIsRaycast(out RaycastHit hitInfo) =>
      Physics
        .Raycast(transform.position, -Vector3.up,
          out hitInfo, TakingOffHeight, RaycastLayer);

    private void TakeOff()
    {
      IsOnGround = false;
      TookOff?.Invoke();
    }

    private void LandOnGround()
    {
      IsOnGround = true;
      LandedOnGround?.Invoke();
    }

    private void CacheLandInfo(RaycastHit hitInfo) => 
      LandInfo = hitInfo;
  }
}