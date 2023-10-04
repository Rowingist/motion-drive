using System;
using System.Collections;
using CodeBase.HeroCar;
using CodeBase.Logic.Bezier;
using UnityEngine;

namespace CodeBase.EnemyCar
{
  public class EnemyCarFollowingTarget : MonoBehaviour
  {
    public Rigidbody Rigidbody;
    public Transform target;
    public float SnapYOffset = 0.85f;

    public LayerMask SnapToMask;

    public HeroCarOnGroundChecker GroundChecker;

    private float _lastPositionY;

    private bool _isSnappingToGround = true;

    private Vector3 _currentPosition;

    private void FixedUpdate()
    {
      if (!target)
        return;

      if (!_isSnappingToGround)
        return;

      _currentPosition = new Vector3(target.position.x, _lastPositionY, target.position.z);
      if (Physics.Raycast(Rigidbody.position + Vector3.up * 2, Vector3.down, out RaycastHit hitInfo, 1000,
            SnapToMask))
      {
        _currentPosition.y = hitInfo.point.y + SnapYOffset;
        _lastPositionY = _currentPosition.y;
      }

      Rigidbody.MovePosition(_currentPosition);
    }

    public void StartSnapping() =>
      _isSnappingToGround = true;

    private IEnumerator CheckingToStartSnapping()
    {
      yield return new WaitUntil(() => GroundChecker.IsOnGround);
      target.GetComponent<SplineWalker>().MakeLastPointUnreachable();
      StartSnapping();
    }

    public void StopSnapping()
    {
      _isSnappingToGround = false;
      Invoke(nameof(StartCheckingRoutine), 1f);
    }

    private void StartCheckingRoutine()
    {
      StartCoroutine(CheckingToStartSnapping());
    }
  }
}