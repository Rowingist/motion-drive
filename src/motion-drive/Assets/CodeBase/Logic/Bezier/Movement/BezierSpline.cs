using System;
using UnityEngine;

namespace CodeBase.Logic.Bezier.Movement
{
  public class BezierSpline : MonoBehaviour
  {
    [SerializeField] private Vector3[] points;

    [SerializeField] private BezierControlPointMode[] _modes;

    [SerializeField] private bool _loop;
    
    public int ControlPointCount =>
      points.Length;

    public Vector3 GetControlPoint(int index) =>
      points[index];

    public void SetControlPoint(int index, Vector3 point)
    {
      if (index % 3 == 0)
      {
        Vector3 delta = point - points[index];

        if (_loop)
        {
          if (index == 0)
          {
            points[1] += delta;
            points[^2] += delta;
            points[^1] = point;
          }
          else if (index == points.Length - 1)
          {
            points[0] = point;
            points[1] += delta;
            points[index - 1] += delta;
          }
          else
          {
            points[index - 1] += delta;
            points[index + 1] += delta;
          }
        }
        else
        {
          if (index > 0)
          {
            points[index - 1] += delta;
          }

          if (index + 1 < points.Length)
          {
            points[index + 1] += delta;
          }
        }
      }
      
      points[index] = point;
      EnforceMode(index);
    }

    public int CurveCount =>
      (points.Length - 1) / 3;

    public Vector3 GetPoint(float t)
    {
      int i;
      if (t >= 1)
      {
        t = 1f;
        i = points.Length - 4;
      }
      else
      {
        t = Mathf.Clamp01(t) * CurveCount;
        i = (int)t;
        t -= i;
        i *= 3;
      }

      return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
      int i;
      if (t >= 1)
      {
        t = 1f;
        i = points.Length - 4;
      }
      else
      {
        t = Mathf.Clamp01(t) * CurveCount;
        i = (int)t;
        t -= i;
        i *= 3;
      }

      return transform.TransformPoint(
        Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t) - transform.position);
    }

    public Vector3 GetDirection(float t)
    {
      return GetVelocity(t).normalized;
    }

    public void Reset()
    {
      points = new Vector3[]
      {
        new Vector3(1f, 0f, 0f),
        new Vector3(2f, 0f, 0f),
        new Vector3(3f, 0f, 0f),
        new Vector3(4f, 0f, 0f)
      };

      _modes = new BezierControlPointMode[]
      {
        BezierControlPointMode.Free,
        BezierControlPointMode.Free
      };
    }

    public void AddCurve()
    {
      Vector3 point = points[^1];
      Array.Resize(ref points, points.Length + 3);
      point.x += 1f;
      points[^3] = point;
      point.x += 1f;
      points[^2] = point;
      point.x += 1f;
      points[^1] = point;
      
      Array.Resize(ref _modes, _modes.Length + 1);
      _modes[^1] = _modes[^2];
      
      EnforceMode(points.Length - 4);

      if (_loop)
      {
        points[^1] = points[0];
        _modes[^1] = _modes[0];
        EnforceMode(0);
      }
    }

    public BezierControlPointMode GetControlPointMode(int index) => 
      _modes[(index + 1) / 3];

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
      int modeIndex = (index + 1) / 3;
      
      _modes[modeIndex] = mode;

      if (_loop)
      {
        if (modeIndex == 0) 
          _modes[^1] = mode;
        else if (modeIndex == _modes.Length - 1)
          _modes[0] = mode;
      }
      
      EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
      int modeIndex = (index + 1) / 3;

      BezierControlPointMode mode = _modes[modeIndex];
      if (mode == BezierControlPointMode.Free || !_loop && (modeIndex == 0 || modeIndex == _modes.Length - 1))
        return;

      int middleIndex = modeIndex * 3;
      int fixedIndex, enforcedIndex;

      if (index <= middleIndex)
      {
        fixedIndex = middleIndex - 1;
        if (fixedIndex < 0) 
          fixedIndex = points.Length - 2;
        
        enforcedIndex = middleIndex + 1;
        if (enforcedIndex >= points.Length) 
          enforcedIndex = 1;
      }
      else
      {
        fixedIndex = middleIndex + 1;
        if (fixedIndex >= points.Length)
          fixedIndex = 1;
        
        enforcedIndex = middleIndex - 1;
        if (enforcedIndex < 0)
          enforcedIndex = points.Length - 2;
      }

      Vector3 middle = points[middleIndex];
      Vector3 enforcedTangent = middle - points[fixedIndex];

      if (mode == BezierControlPointMode.Aligned)
      {
        enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
      }
      
      points[enforcedIndex] = middle + enforcedTangent;
    }

    public bool Loop
    {
      get { return _loop; }
      set
      {
        _loop = value;
        if (value)
        {
          _modes[^1] = _modes[0];
          SetControlPoint(0, points[0]);
        }
      }
    }
  }
}