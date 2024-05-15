using UnityEditor;
using UnityEngine;

namespace SmartTrackSystem
{
    [CustomEditor(typeof(RecordObjectPositionHelper))]
    public class RecordObjectPositionHelperCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RecordObjectPositionHelper positionHelper = (RecordObjectPositionHelper)target;

            if (GUILayout.Button("Create Setup"))
            {
                positionHelper.CreateSetup();
            }

            if (GUILayout.Button("Clear Setup"))
            {
                positionHelper.ClearSetup(true);
            }
        }
    }
}
