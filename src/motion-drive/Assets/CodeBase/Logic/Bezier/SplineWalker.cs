using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.Events.LevelStart.Subscribers;
using CodeBase.Logic.Bezier.Movement;
using UnityEngine;

namespace CodeBase.Logic.Bezier
{
  public class SplineWalker : OnStartLevelSubscriber
  {
    public TriggerObserver TriggerObserver;

    public float Duration;
    public bool LookForward;

    public SplineWalkerMode Mode;

    private BezierSpline _spline;
    
    private float _progress;
    private bool _goingForward = true;

    private float _lastPositionZ;
    
    public bool IsStarted;

    public void Construct(BezierSpline spline) => 
      _spline = spline;

    private void Start()
    {
      TriggerObserver.TriggerEnter += UpdateLastTrampolineTargetPointZ;
      MakeLastPointUnreachable();
    }

    private void OnDestroy() => 
      TriggerObserver.TriggerEnter -= UpdateLastTrampolineTargetPointZ;

    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      IsStarted = true;

    private void UpdateLastTrampolineTargetPointZ(Collider obj)
    {
      if (obj.gameObject.layer == LayerMask.NameToLayer("Trampoline"))
        _lastPositionZ = obj.GetComponentInParent<Trampoline.Trampoline>().LandingPoint.position.z;
    }

    private void Update()
    {
      if(!IsStarted)
        return;

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

      Vector3 position = _spline.GetPoint(_progress);
      transform.localPosition = position;
      
      if (LookForward)
      {
        transform.LookAt(position + _spline.GetDirection(_progress));
      }
    }

    public void MakeLastPointUnreachable()
    {
      _lastPositionZ = float.MaxValue;
    }
  }
}