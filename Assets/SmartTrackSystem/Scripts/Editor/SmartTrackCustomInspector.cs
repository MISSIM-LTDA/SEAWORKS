using UnityEditor;
using UnityEngine;

namespace SmartTrackSystem 
{
    [CustomEditor(typeof(SmartTrack))]
    public class SmartTrackCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(20);

            SmartTrack smartTrack = (SmartTrack)target;

            if (GUILayout.Button("Create Setup"))
            {
                smartTrack.CreateSetUp();
            }

            if (GUILayout.Button("Clear Setup"))
            {
                smartTrack.ClearSetup(true);
            }
        }
    }
}
