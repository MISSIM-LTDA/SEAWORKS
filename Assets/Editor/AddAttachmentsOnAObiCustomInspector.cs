using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddAttachmentsOnAObi))]
public class AddAttachmentsOnAObiCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AddAttachmentsOnAObi addAttachments = (AddAttachmentsOnAObi)target;

        if (GUILayout.Button("Add Attachments in Lock Particles"))
        {
            addAttachments.AddLock();
        }

        if (GUILayout.Button("Add Attachments in Break Particles"))
        {
            addAttachments.AddBreak();
        }

        if (GUILayout.Button("Remove Lock Particles Attachments"))
        {
            addAttachments.RemoveLocks();
        }

        if (GUILayout.Button("Remove Break Particles Attachments"))
        {
            addAttachments.RemoveBreaks();
        }

        if (GUILayout.Button("Remove All Particles Attachments"))
        {
            addAttachments.RemoveAll();
        }
    }
}