using CodeBase.Logic.CheckPoint;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(CheckPointMarker))]

  public class CheckPointMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(CheckPointMarker point, GizmoType gizmo)
    {
      Gizmos.color = new Color(0, 1, 0, 0.98f);
      Gizmos.DrawSphere(point.transform.position, 0.25f);
    }
  }
}