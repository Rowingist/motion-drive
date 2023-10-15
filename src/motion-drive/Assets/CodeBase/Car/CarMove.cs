using Unity.Mathematics;
using UnityEngine;

namespace CodeBase.Car
{
  public class CarMove : MonoBehaviour
  {
    private const float MaxAngularVelocity = 1000f;

    public CarOnGroundChecker GroundChecker;

    public Rigidbody SelfRigidbody;
    public float RotateSpeed;
    public float RotateDelay;

    private Vector3 _lastPosition;
    private float _rotateSpeedOnStart;

    private Rigidbody _followingRigidbody;

    public MoveType MoveType = MoveType.Player;

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
      Vector3 lookDirection = new Vector3();

      lookDirection = MoveType == MoveType.Player
        ? _followingRigidbody.velocity
        : _followingRigidbody.position - transform.position;

      Quaternion lookFollowingRotation = quaternion.identity;

      if (lookDirection != Vector3.zero)
        lookFollowingRotation = Quaternion.LookRotation(lookDirection);

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