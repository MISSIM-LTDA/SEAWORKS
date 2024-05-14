using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FixedUpdaterConfigure))]
public class ObiFixedUpdaterConfigureCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FixedUpdaterConfigure fixedUpdaterConfigure = (FixedUpdaterConfigure)target;

        if (GUILayout.Button("FixScene"))
        {
            fixedUpdaterConfigure.FixScene();
        }
    }
}