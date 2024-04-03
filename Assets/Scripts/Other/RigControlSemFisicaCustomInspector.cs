using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RigControlSemFisica))]
[ExecuteInEditMode]
public class RigControlSemFisicaCustomInspector : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RigControlSemFisica RigControl = (RigControlSemFisica)target;
        if (GUILayout.Button("Go to Normal Angles"))
        {
            RigControl.GoToNormalAngles();
        }
       
    }
#endif
}
