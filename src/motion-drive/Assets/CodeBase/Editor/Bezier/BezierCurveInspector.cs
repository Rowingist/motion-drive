using CodeBase.Logic.Bezier;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.Bezier
{
  [CustomEditor(typeof(BezierCurve))]
  public class BezierCurveInspector : UnityEditor.Editor
  {
    private const int LineSteps = 10;
    private const float DirectionScale = 0.5f;
    
    private BezierCurve _curve;
    private Transform _handleTransform;
    private Quaternion _handleRotation;

    private void OnSceneGUI()
    {
      _curve = target as BezierCurve;

      _handleTransform = _curve.transform;
      _handleRotation =
        Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;

      Vector3 p0 = ShowPoint(0);
      Vector3 p1 = ShowPoint(1);
      Vector3 p2 = ShowPoint(2);
      Vector3 p3 = ShowPoint(3);

      Handles.color = Color.gray;
      Handles.DrawLine(p0, p1);
      Handles.DrawLine(p2, p3);

      ShowDirections();
      Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
    }

    private void ShowDirections()
    {
      Handles.color = Color.green;
      Vector3 point = _curve.GetPoint(0f);
      Handles.DrawLine(point, point + _curve.GetDirection(0f) * DirectionScale);
      for (int i = 0; i <= LineSteps; i++)
      { 
        point = _curve.GetPoint(i / (float)LineSteps);
        Handles.DrawLine(point, point + _curve.GetDirection(i / (float)LineSteps) * DirectionScale);
      }
    }

    private Vector3 ShowPoint(int index)
    {
      Vector3 point = _handleTransform.TransformPoint(_curve.points[index]);

      EditorGUI.BeginChangeCheck();
      point = Handles.DoPositionHandle(point, _handleRotation);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(_curve, "Move Point");
        EditorUtility.SetDirty(_curve);
        _curve.points[index] = _handleTransform.InverseTransformPoint(point);
      }

      return point;
    }
  }
}