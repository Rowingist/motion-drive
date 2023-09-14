using System;
using UnityEngine;

namespace CodeBase.HeroCar
{
  public class HeroCarMove : MonoBehaviour
  {
    private const float MaxAngularVelocity = 1000f;
    
    public HeroCarOnGroundChecker GroundChecker;
    
    public Rigidbody SelfRigidbody;
    public float RotateSpeed;
    public float RotateDelay;

    private Vector3 _lastPosition;
    private float _rotateSpeedOnStart;
    
    private Rigidbody _followingRigidbody;

    public void Construct(Rigidbody followingRigidbody) => 
      _followingRigidbody = followingRigidbody;

    private void Start()
    {
      _rotateSpeedOnStart = RotateSpeed;
      GroundChecker.TookOff += ResetRotateSpeed;
      SelfRigidbody.maxAngularVelocity = MaxAngularVelocity;
    }

    private void FixedUpdate()
    {
      if(!_followingRigidbody) return;
      
      MoveRigidbody();
      
      if(!VelocityIsHigh()) return;
      if(!GroundChecker.IsOnGround) return;
      
      RotateRigidbody();
      CacheLastPosition();
    }

    private void MoveRigidbody() => 
      SelfRigidbody.MovePosition(_followingRigidbody.position);

    private void RotateRigidbody()
    {
      Quaternion lookFollowingRotation = Quaternion.LookRotation(_followingRigidbody.velocity);
      SelfRigidbody.rotation =
        Quaternion.RotateTowards(SelfRigidbody.rotation, lookFollowingRotation, RotateSpeed * Time.deltaTime);
    }

    private void CacheLastPosition() => 
      _lastPosition = SelfRigidbody.position;

    private bool VelocityIsHigh() => 
      _followingRigidbody.velocity.magnitude > RotateDelay;

    private void ResetRotateSpeed() => 
      RotateSpeed = _rotateSpeedOnStart;

    private void SetRotateSpeed(float value) => 
      RotateDelay = value;
  }
}