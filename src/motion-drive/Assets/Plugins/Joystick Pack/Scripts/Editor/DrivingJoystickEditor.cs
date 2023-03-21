using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEditor;
using UnityEngine;

namespace Plugins.Joystick_Pack.Scripts.Editor
{
  [CustomEditor(typeof(DrivingJoystick))]
  public class DrivingJoystickEditor : JoystickEditor
  {
    private SerializedProperty _moveThreshold;

    protected override void OnEnable()
    {
      base.OnEnable();
      _moveThreshold = serializedObject.FindProperty("_moveThreshold");
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (background != null)
      {
        RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
        backgroundRect.pivot = center;
      }
    }

    protected override void DrawValues()
    {
      base.DrawValues();
      EditorGUILayout.PropertyField(_moveThreshold,
        new GUIContent("Move Threshold",
          "The distance away from the center input has to be before the joystick begins to move."));
    }
  }
}