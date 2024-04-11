using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PaintParticleGroupsRenders))]
public class PaintParticleGroupsRendersCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PaintParticleGroupsRenders paintParticleGroupsRenders = (PaintParticleGroupsRenders)target;
    }
}
