using CodeBase.Logic.CameraSwitchPoint;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(CameraSwitchPointMarker))]
  public class CameraSwitchPointMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(CameraSwitchPointMarker point, GizmoType gizmo)
    {
      Gizmos.color = new Color(0, 0, 0.8f, 0.98f);
      Gizmos.DrawSphere(point.transform.position, 0.25f);
    }
  }
}