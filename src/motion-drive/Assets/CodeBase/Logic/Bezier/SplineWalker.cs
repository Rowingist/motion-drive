using System;
using CodeBase.Logic.Bezier.Movement;
using UnityEngine;

namespace CodeBase.Logic.Bezier
{
  public class SplineWalker : MonoBehaviour
  {
    public BezierSpline Spline;
    public TriggerObserver TriggerObserver;

    public float Duration;
    public bool LookForward;

    public SplineWalkerMode Mode;

    private float _progress;
    private bool _goingForward = true;

    private float _lastPositionZ;

    private void Start()
    {
      TriggerObserver.TriggerEnter += UpdateLastTrampolineTargetPointZ;
      MakeLastPointUnreachable();
    }

    private void OnDestroy() => 
      TriggerObserver.TriggerEnter -= UpdateLastTrampolineTargetPointZ;

    private void UpdateLastTrampolineTargetPointZ(Collider obj)
    {
      print("Got");
      
      if (obj.gameObject.layer == LayerMask.NameToLayer("Trampoline"))
        _lastPositionZ = obj.GetComponentInParent<Trampoline.Trampoline>().LandingPoint.position.z;
    }

    private void Update()
    {
      if(transform.position.z >= _lastPositionZ)
        return;

      if (_goingForward)
      {
        _progress += Time.deltaTime / Duration;
        
        if (_progress > 1f)
        {
          switch (Mode)
          {
            case SplineWalkerMode.Once:
              _progress = 1f;
              break;
            case SplineWalkerMode.Loop:
              _progress -= 1f;
              break;
            default:
              _progress = 2f - _progress;
              _goingForward = false;
              break;
          }
        }
      }
      else
      {
        _progress -= Time.deltaTime / Duration;
        
        if (_progress < 0f)
        {
          _progress = -_progress;
          _goingForward = true;
        }
      }

      Vector3 position = Spline.GetPoint(_progress);
      transform.localPosition = position;
      
      if (LookForward)
      {
        transform.LookAt(position + Spline.GetDirection(_progress));
      }
    }

    public void MakeLastPointUnreachable()
    {
      _lastPositionZ = float.MaxValue;
    }
  }
}