using CodeBase.HeroCar;
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
    private HeroCarOnGroundChecker _groundChecker;

    private void Construct(HeroCarOnGroundChecker onGroundChecker)
    {
      _groundChecker = onGroundChecker;
    }
    
    private void Update()
    {
      if (_groundChecker && _groundChecker.IsOnGround)
      {
        _following.y = InverseSpringTransformPoint();
        transform.localPosition = SmoothFollowing();
      }
    }

    private float InverseSpringTransformPoint() =>
      transform.InverseTransformPoint(Spring.position).y;

    private Vector3 SmoothFollowing() =>
      Vector3.SmoothDamp(transform.localPosition, _following, ref _velocity, SmoothTime, MaxSpeed);
  }
}