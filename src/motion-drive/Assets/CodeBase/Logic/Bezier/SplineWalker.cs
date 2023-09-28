using CodeBase.Logic.Bezier.Movement;
using UnityEngine;

namespace CodeBase.Logic.Bezier
{
  public class SplineWalker : MonoBehaviour
  {
    public BezierSpline Spline;

    public float Duration;
    public bool LookForward;

    public SplineWalkerMode Mode;

    private float _progress;
    private bool _goingForward = true;

    private void Update()
    {
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
  }
}