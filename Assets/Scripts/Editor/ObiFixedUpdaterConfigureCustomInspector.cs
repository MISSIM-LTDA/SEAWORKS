using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FixedUpdaterConfigurator))]
public class ObiFixedUpdaterConfigureCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FixedUpdaterConfigurator fixedUpdaterConfigure = (FixedUpdaterConfigurator)target;

        if (GUILayout.Button("FixScene"))
        {
            fixedUpdaterConfigure.FixScene();
        }
    }
}