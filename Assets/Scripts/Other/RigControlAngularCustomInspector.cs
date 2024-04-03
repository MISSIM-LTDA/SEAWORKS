using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RigControlAngular))]
[ExecuteInEditMode]
public class RigControlAngularCustomInspector : Editor
{
#if UNITY_EDITOR
    bool LockControler;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RigControlAngular RigControl = (RigControlAngular)target;
        if (GUILayout.Button("FlipAllValues"))
        {
            RigControl.FlipAngularDisplacements();
        }
        if (GUILayout.Button("ResetAllValues"))
        {
            RigControl.ResetAngularDisplacements();
        }
        string btn_text = "Lock Controler";
        if (LockControler)
            btn_text = "Unlock Controler";
        if (GUILayout.Button(btn_text))
        {
            RigControl.LockControler = true ? RigControl.LockControler == false:true;
            LockControler = true ? LockControler == false : true;
        }
    }
#endif
}
