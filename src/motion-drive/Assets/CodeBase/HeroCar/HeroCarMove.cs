using System;
using CodeBase.Services.Input;
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

    public Rigidbody _followingRigidbody;

    public MoveType MoveType = MoveType.Player;

    public void Construct(Rigidbody followingRigidbody)
    {
      _followingRigidbody = followingRigidbody;
    }

    private void Start()
    {
      _rotateSpeedOnStart = RotateSpeed;
      GroundChecker.TookOff += ResetRotateSpeed;
      SelfRigidbody.maxAngularVelocity = MaxAngularVelocity;
    }

    private void FixedUpdate()
    {
      if (!_followingRigidbody) return;

      MoveRigidbody();

      if (!VelocityIsHigh()) return;
      if (!GroundChecker.IsOnGround) return;

      RotateRigidbody();
      CacheLastPosition();
    }

    Vector3 _currentSpeed = Vector3.zero;
    public float Speed;

    private void MoveRigidbody() =>
      SelfRigidbody.position =
        Vector3.SmoothDamp(SelfRigidbody.position, _followingRigidbody.position, ref _currentSpeed, Speed);

    private void RotateRigidbody()
    {
      Quaternion lookFollowingRotation;
      lookFollowingRotation = MoveType == MoveType.Player
        ? Quaternion.LookRotation(_followingRigidbody.velocity)
        : Quaternion.LookRotation(_followingRigidbody.position - transform.position);

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