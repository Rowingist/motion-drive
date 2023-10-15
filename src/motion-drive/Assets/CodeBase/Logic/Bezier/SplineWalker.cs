using CodeBase.Infrastructure.Events;
using CodeBase.Infrastructure.Events.LevelStart.Subscribers;
using CodeBase.Logic.Bezier.Movement;
using UnityEngine;

namespace CodeBase.Logic.Bezier
{
  public class SplineWalker : OnStartLevelSubscriber
  {
    public Rigidbody SelfBody;

    public float MaxDuration;
    public bool LookForward;

    public SplineWalkerMode Mode;

    private BezierSpline _spline;

    private float _currentDuration;
    
    private float _progress;
    private bool _goingForward = true;

    private float _lastPositionZ;
    
    public bool IsRunning;

    private float _cachedProgress;
    
    public void Construct(BezierSpline spline, float duration)
    {
      _spline = spline;
      _currentDuration = duration;
    }

    private void Start()
    {
      MakeLastPointUnreachable();
      CacheCurrentProgress();
    }

    protected override void OnLevelStarted(CurrentLevelStartInfo levelStartInfo) => 
      IsRunning = true;

    private void Update()
    {
      if(!IsRunning)
        return;

      if(transform.position.z >= _lastPositionZ)
        return;

      if (_goingForward)
      {
        _progress += Time.deltaTime / _currentDuration;
        
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
        _progress -= Time.deltaTime / _currentDuration;
        
        if (_progress < 0f)
        {
          _progress = -_progress;
          _goingForward = true;
        }
      }

      Vector3 position = _spline.GetPoint(_progress);
      SelfBody.MovePosition(position);
      
      AlignStraight(position);
    }

    private void AlignStraight(Vector3 position)
    {
      if (LookForward)
        transform.LookAt(position + _spline.GetDirection(_progress));
    }

    public void MakeLastPointUnreachable() => 
      _lastPositionZ = float.MaxValue;

    public void StopMovement() => 
      IsRunning = false;

    public void StartMovement() => 
      IsRunning = true;

    public void BackToCachedProgress() => 
      _progress = _cachedProgress;

    public void CacheCurrentProgress() => 
      _cachedProgress = _progress;

    public void ChangeDuration(float value)
    {
      if(_currentDuration - value < MaxDuration)
        return;
      
      print("update");
      
      _currentDuration -= value;
    }

    public void ChangeLastPositionZ(float currentValue)
    { 
      if(_lastPositionZ + currentValue < _lastPositionZ)
        return;

      _lastPositionZ = currentValue;
    }
  }
}