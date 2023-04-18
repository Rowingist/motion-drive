using CodeBase.Services.Input;
using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace CodeBase.HeroFollowingTarget
{
  [RequireComponent(typeof(Rigidbody))]
  public class HeroFollowingTarget : MonoBehaviour
  {
    private const float FallSmoothTime = 10f;

    [Header("Movement settings")] [SerializeField, Range(0f, 100f)]
    private float _maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 10f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 1f;
    [SerializeField, Range(0f, 90f)] private float _maxGroundAngle = 25f;
    [SerializeField, Range(0f, 100f)] private float _horizontalForce = 0;
    [SerializeField] private AnimationCurve _horizontalDragPerVelocity;
    [SerializeField] private float _aligningSpeed = 4f;

    [Header("Snapping on surface")] [SerializeField, Range(0f, 200f)]
    private float _maxSnapOnSurfaceSpeed = 100f;

    [SerializeField, Min(0f)] private float _distanceToBeginSnap = 1f;
    [SerializeField] private LayerMask _snapSurfaceMask = -1;
    [SerializeField] private float _snapToGroundSpeed = 1;
    
    private Rigidbody _rigidbody;
    private IInputService _playerInput;

    private Vector3 _velocity;
    private Vector3 _desiredVelocity;
    private Vector3 _contactNormal;
    private Vector3 _steepNormal;

    private int _groundContactCount;
    private int _steepContactCount;
    private int _stepsSinceLastGrounded;

    private float _minGroundDotProduct;
    private float _minStairsDotProduct;
    private float _currentYVelocity;
    private bool _isSnapping;

    private bool OnGround => _groundContactCount > 0;

    public void Construct(IInputService input) => 
      _playerInput = input;

    private void OnValidate() =>
      _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);

    private void Awake()
    {
      _rigidbody = GetComponent<Rigidbody>();
      EnableSnapping();
    }

    private void FixedUpdate()
    {
      UpdateState();
      AdjustVelocity();

      BindSpeedHorizontalDragSensitivity();

      if (!OnGround)
      {
        SpeedUpLanding();
        return;
      }

      _rigidbody.velocity = _velocity;
      ClearState();
    }

    public void DisableSnapping() => 
      _isSnapping = false;   
    public void EnableSnapping() => 
      _isSnapping = true;

    private void SpeedUpLanding()
    {
      if (_velocity.y < 0)
        _velocity.y = Mathf.SmoothDamp(_velocity.y, -_velocity.z * 2, ref _currentYVelocity, FallSmoothTime);
    }

    private void BindSpeedHorizontalDragSensitivity()
    {
      float k = _velocity.z / _maxSpeed;
      _horizontalForce = _horizontalDragPerVelocity.Evaluate(k);
    }

    private void OnCollisionEnter(Collision collision) =>
      EvaluateCollision(collision);

    private void OnCollisionStay(Collision collision) =>
      EvaluateCollision(collision);

    private void Update()
    {
      if(_playerInput == null)
        return;

      Vector2 playerInput;
      playerInput.x = _playerInput.IsFingerHoldOnScreen() ? _playerInput.Axis.x : 0;
      playerInput.y = _playerInput.IsFingerHoldOnScreen() ? 1 : 0;
      playerInput = Vector2.ClampMagnitude(playerInput, 1f);

      _desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * _maxSpeed;
    }

    private void EvaluateCollision(Collision collision)
    {
      float minDot = GetMinDot(collision.gameObject.layer);

      for (int i = 0; i < collision.contactCount; i++)
      {
        Vector3 normal = collision.GetContact(i).normal;
        if (normal.y >= minDot)
        {
          _groundContactCount += 1;
          _contactNormal += normal;
        }
        else if (normal.y > -0.01f)
        {
          _steepContactCount += 1;
          _steepNormal += normal;
        }
      }
    }

    private float GetMinDot(int layer) =>
      ((1 << layer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;

    private void UpdateState()
    {
      _velocity = _rigidbody.velocity;

      if (OnGround || SnapToGround() || CheckSteepContacts())
      {
        _stepsSinceLastGrounded = 0;

        if (_groundContactCount > 1)
          _contactNormal.Normalize();
      }
    }

    private bool SnapToGround()
    {
      if (!_isSnapping)
        return false;
      
      if (_stepsSinceLastGrounded > 1)
        return false;

      if (_velocity.magnitude > _maxSnapOnSurfaceSpeed)
        return false;

      if (!Physics.Raycast(_rigidbody.position, Vector3.down, out RaycastHit hit, _distanceToBeginSnap,
            _snapSurfaceMask))
        return false;

      if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        return false;

      _groundContactCount = 1;
      _contactNormal = hit.normal;

      if (Vector3.Dot(_velocity, hit.normal) > 0f)
        _velocity = (_velocity - hit.normal * (Vector3.Dot(_velocity, hit.normal) * _snapToGroundSpeed)).normalized *
                    _velocity.magnitude;

      return true;
    }

    private bool CheckSteepContacts()
    {
      if (_steepContactCount > 1)
      {
        _steepNormal.Normalize();

        if (_steepNormal.y >= _minGroundDotProduct)
        {
          _steepContactCount = 0;
          _groundContactCount = 1;
          _contactNormal = _steepNormal;
          return true;
        }
      }

      return false;
    }

    private void AdjustVelocity()
    {
      Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
      Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

      float currentX = Vector3.Dot(_velocity, xAxis);
      float currentZ = Vector3.Dot(_velocity, zAxis);

      float acceleration = OnGround ? _maxAcceleration : _maxAirAcceleration;
      float maxHorizontalSpeedChange = _horizontalForce * Time.deltaTime;
      float maxSpeedChange = acceleration * Time.deltaTime;

      float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxHorizontalSpeedChange);
      float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

      _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector) =>
      vector - _contactNormal * Vector3.Dot(vector, _contactNormal);
    
    private void ClearState()
    {
      _groundContactCount = 0;
      _steepContactCount = 0;
      _contactNormal = Vector3.zero;
      _steepNormal = Vector3.zero;
    }
  }
}