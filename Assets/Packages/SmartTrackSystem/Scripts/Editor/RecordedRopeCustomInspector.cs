using UnityEditor;
using UnityEngine;

namespace SmartTrackSystem
{
    [CustomEditor(typeof(RecordedRope))]
    public class RecordedRopeCustomInspector : Editor
    {
        SerializedProperty recordRope;
        SerializedProperty recordRopePosition;
        RecordedRope recordedRope;

        private void OnEnable()
        {
            recordedRope = target as RecordedRope;
            recordRope = serializedObject.FindProperty("recordRope");

            recordRopePosition = serializedObject.FindProperty("recordRopePosition");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(recordRope, new GUIContent("Record Rope"));

            EditorGUILayout.PropertyField(recordRopePosition, new GUIContent("Record Rope Position"));

            if (GUI.changed) { serializedObject.ApplyModifiedProperties(); }
        }
    }
}
