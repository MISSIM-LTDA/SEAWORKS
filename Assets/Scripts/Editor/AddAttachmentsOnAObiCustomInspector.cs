using UnityEngine;
using UnityEditor;
using static Obi.ObiParticleAttachment;

[CustomEditor(typeof(AddAttachmentsOnAObi))]
public class AddAttachmentsOnAObiCustomInspector : Editor
{
    SerializedProperty objectToAttach;
    SerializedProperty rope;
    SerializedProperty particleGroupName;
    SerializedProperty attachmentType;
    SerializedProperty compliance;
    SerializedProperty breakThreshold;

    AddAttachmentsOnAObi addAttachment;
    private void OnEnable()
    {
        addAttachment = target as AddAttachmentsOnAObi;

        objectToAttach = serializedObject.FindProperty("objectToAttach");

        rope = serializedObject.FindProperty("rope");

        particleGroupName = serializedObject.FindProperty("particleGroupName");

        attachmentType = serializedObject.FindProperty("attachmentType");

        compliance = serializedObject.FindProperty("compliance");
        breakThreshold = serializedObject.FindProperty("breakThreshold");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(objectToAttach, new GUIContent("Object To Attach"));

        EditorGUILayout.Space(20);

        EditorGUILayout.PropertyField(rope, new GUIContent("Rope", "Place the rope object you want to attach the particles"));

        if (addAttachment.objectToAttach && addAttachment.rope) {
            EditorGUILayout.PropertyField(particleGroupName, new GUIContent("Particle Group Name",
                "Fill with the name of the particles you want to add lock attachments"));
            EditorGUILayout.PropertyField(attachmentType, new GUIContent("Attachment Type"));

            if(addAttachment.attachmentType == AttachmentType.Dynamic) {
                EditorGUILayout.PropertyField(compliance, new GUIContent("Compliance"));
                EditorGUILayout.PropertyField(breakThreshold, new GUIContent("BreakThreshold"));
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Add Attachments")) {
                addAttachment.AddAttachments();
            }

            if (GUILayout.Button("Remove Attachments")) {
                addAttachment.RemoveAttachments();
            }
        }

        if (GUI.changed) { serializedObject.ApplyModifiedProperties(); }
    }
}