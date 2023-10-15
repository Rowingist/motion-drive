using System.Collections;
using CodeBase.Car;
using CodeBase.Logic.Bezier;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyFollowingTarget : MonoBehaviour
  {
    public Rigidbody Rigidbody;
    public float SnapYOffset = 0.85f;

    public LayerMask MaskToSnap;

    private Transform _target;
    private CarOnGroundChecker _groundChecker;
    private SplineWalker _splineWalker;

    private float _lastPositionY;

    private bool _isSnappingToGround = true;

    private Vector3 _currentPosition;

    public SplineWalker SplineWalker => _splineWalker;

    public void Construct(Transform target)
    {
      _target = target;
      _splineWalker = _target.GetComponent<SplineWalker>();
    }

    public void Construct(CarOnGroundChecker groundChecker) => 
      _groundChecker = groundChecker;

    private void FixedUpdate()
    {
      if (!_target)
        return;

      if (!_isSnappingToGround)
        return;

      _currentPosition = new Vector3(_target.position.x, _lastPositionY, _target.position.z);
      if (Physics.Raycast(Rigidbody.position + Vector3.up * 2, Vector3.down, out RaycastHit hitInfo, 1000,
            MaskToSnap))
      {
        _currentPosition.y = hitInfo.point.y + SnapYOffset;
        _lastPositionY = _currentPosition.y;
      }

      Rigidbody.MovePosition(_currentPosition);
    }

    private IEnumerator CheckingToStartSnapping()
    {
      yield return new WaitUntil(() => _groundChecker.IsOnGround);
      
      ContinueMovementBySpline();
      
      StartSnapping();
    }

    private void ContinueMovementBySpline() => 
      _splineWalker.MakeLastPointUnreachable();

    private void StartSnapping() =>
      _isSnappingToGround = true;

    public void StopSnapping()
    {
      _isSnappingToGround = false;
      Invoke(nameof(StartCheckingRoutine), 1f);
    }

    private void StartCheckingRoutine() => 
      StartCoroutine(CheckingToStartSnapping());

    public void ChangeSpeed(float value)
    {
      _splineWalker.ChangeDuration(value);
    }
  }
}