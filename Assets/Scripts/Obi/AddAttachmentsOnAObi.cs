using System.Collections.Generic;
using UnityEngine;
using Obi;
using static Obi.ObiParticleAttachment;

[ExecuteInEditMode]
public class AddAttachmentsOnAObi : MonoBehaviour
{
    public GameObject objectToAttach;
    public ObiRopeBase rope;

    //Name on particle groups to look for
    public string particleGroupName;

    public AttachmentType attachmentType;

    [SerializeField] private float breakThreshold;
    [SerializeField] private float compliance;
    private void Awake()
    {
        objectToAttach = gameObject;

        particleGroupName = "None";
    }

    //Add Attachments on the specific named group particles
    public void AddAttachments()
    {
        if (particleGroupName == "None"){
            Debug.Log("Choose a name to search for in" +
                " ObiActor and add Attachments");

            return;
        }

        ObiParticleAttachment[] particleAttachments = 
            rope.GetComponents<ObiParticleAttachment>();

        List<ObiParticleGroup> particleGroups = rope.sourceBlueprint.groups;

        int nameCounter = 0;
        int numberOfParticleAttachments = particleAttachments.Length;

        for (int i = 0; i < particleGroups.Count; i++) {
            if (rope.path.GetName(i).Contains(particleGroupName)) {
                nameCounter++;

                string groupName = particleGroupName + i;
                rope.path.SetName(i, groupName);

                bool add = true;

                if (particleAttachments.Length != 0) {
                    for (int j = 0; j < numberOfParticleAttachments; j++) {
                        if (particleAttachments[j].particleGroup == particleGroups[i]) {
                            add = false;

                            break;
                        }
                    }
                }

                if (add) {
                    ObiParticleAttachment attachment = 
                        rope.gameObject.AddComponent<ObiParticleAttachment>();

                    attachment.target = objectToAttach.transform;
                    attachment.particleGroup = particleGroups[i];
                    attachment.attachmentType = attachmentType;

                    if (attachmentType == AttachmentType.Dynamic) {
                        attachment.compliance = compliance;
                        attachment.breakThreshold = breakThreshold;

                    }
                }
            }            
        }

        if (nameCounter == 0) {
            Debug.Log("Don't have any particle group with" +
                " this name to put lock attachment");
            return;
        }

        Debug.Log("Obi Attachments added!");
    }

    //Remove Attachments on the specific named group particles
    public void RemoveAttachments()
    {
        int nameCounter = 0;

        ObiParticleAttachment[] particleAttachments =
            rope.GetComponents<ObiParticleAttachment>();

        int numberOfParticleAttachments = 
            particleAttachments.Length;

        if (numberOfParticleAttachments == 0) {
            Debug.Log("Don't have any attachment to erase");

            return;
        }

        for (int i = 0; i < numberOfParticleAttachments; i++) { 
            ObiParticleAttachment attachment = particleAttachments[i];

            if (attachment.attachmentType == attachmentType && 
                attachment.particleGroup.name.Contains(particleGroupName)) {

                nameCounter++;
                DestroyImmediate(attachment);
            }
        }

        if (nameCounter == 0) {
            Debug.Log("Don't have any attachments" +
                " with this name to erase");

            return;
        }

        Debug.Log("Obi Attachments erased!");
    }
}



