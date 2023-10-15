using CodeBase.Car;
using UnityEngine;

namespace CodeBase.Logic.CarParts
{
  public class SpringMovementRepeater : MonoBehaviour
  {
    public Transform Spring;
    public float SmoothTime;
    public float MaxSpeed;

    private Vector3 _following;
    private Vector3 _velocity;
    private CarOnGroundChecker _groundChecker;

    public void Construct(CarOnGroundChecker onGroundChecker)
    {
      _groundChecker = onGroundChecker;
    }

    private void Update()
    {
      if (_groundChecker && _groundChecker.IsOnGround)
      {
        _following.y = InverseSpringTransformPoint();
        transform.localPosition = SmoothFollowing(_following);
      }
      else
      {
        transform.localPosition = SmoothFollowing(Vector3.zero);
      }
    }

    private float InverseSpringTransformPoint() =>
      transform.InverseTransformPoint(Spring.position).y;

    private Vector3 SmoothFollowing(Vector3 localPosition) =>
      Vector3.SmoothDamp(transform.localPosition, localPosition, ref _velocity, SmoothTime, MaxSpeed);
  }
}