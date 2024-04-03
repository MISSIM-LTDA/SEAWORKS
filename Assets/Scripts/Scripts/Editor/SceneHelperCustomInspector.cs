using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneHelper))]
public class SceneHelperCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneHelper sceneHelper = (SceneHelper)target;

        if (GUILayout.Button("Hide and Unhide")) { sceneHelper.HideUnHideGameObject(); }
    }
}
