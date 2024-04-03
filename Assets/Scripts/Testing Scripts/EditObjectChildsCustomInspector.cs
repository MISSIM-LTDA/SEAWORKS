using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditObjectChilds))]
[ExecuteInEditMode]
public class EditObjectChildsCustomInspector : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditObjectChilds editChildObjects = (EditObjectChilds)target;

        if (GUILayout.Button("Turn On/Off Colliders"))
        {
            editChildObjects.TurOnOffColliders();
        }

        if (GUILayout.Button("Turn On/Off Baked Render"))
        {
            editChildObjects.TurOnOffBaked();
        }
    }
#endif
}

