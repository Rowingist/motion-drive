using CodeBase.Logic.MovementSettingsChangePoint;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(MovementSettingsPointMarker))]
  public class MovementSettingPointMarkerEditor : UnityEditor.Editor
  {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(MovementSettingsPointMarker point, GizmoType gizmo)
    {
      Gizmos.color = new Color(1, 1, 1, 0.98f);
      Gizmos.DrawSphere(point.transform.position, 1f);
    }
  }
}