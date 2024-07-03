using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

[ExecuteInEditMode]
public class AddAttachmentsOnAObi : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Place the rope object you want to attach the particles")]
    private GameObject rope;
    private ObiActor obiActor;
    private ObiRopeBlueprintBase bluePrint;

    private List<ObiParticleGroup> particleGroups = new List<ObiParticleGroup>() { };
    private ObiParticleAttachment[] particleAttachments;
    private int numberOfParticleAttachments;

    [SerializeField]
    private GameObject objectToAttach;

    [SerializeField]
    private float breakThreshold;
    [SerializeField]
    private float compliance;

    //Name on particle groups to look for
    [Tooltip("Fill with the name of the particles you want to add lock attachments")]
    [SerializeField] string particleToLockStaticName;
    [Tooltip("Fill with the name of the particles you want to add breakble attachments")]
    [SerializeField] string particleToLockDynamicName;

    private void Awake()
    {
        objectToAttach = this.gameObject;

        particleToLockStaticName = "None";
        particleToLockDynamicName = "None";
    }

    //Runs every times an Add or Remove funciton is called
    //Certifies that the object to be attached and the obiActor were previously chosen
    public bool Initialize(bool adding)
    {
        if (adding) 
        {
            if (objectToAttach == null)
            {
                Debug.Log("Choose a object to attach the group particles on the rope");
                return false;
            }
        }

        if (rope == null)
        {
            Debug.Log("Place the rope object on the inspector before");
            return false;
        }

        obiActor = rope.GetComponent<ObiActor>();

        if (obiActor == null)
        {
            Debug.Log("The choosed object have no actor, choose another one");
            return false;
        }

        if (obiActor.GetType() == typeof(ObiRope))
        {
            ObiRope obiRope = rope.GetComponent<ObiRope>();
            bluePrint = obiRope.ropeBlueprint;
        }

        else
        {
            ObiRod obiRod = rope.GetComponent<ObiRod>();
            bluePrint = obiRod.rodBlueprint;
        }

        particleGroups = bluePrint.groups;

        particleAttachments = obiActor.GetComponents<ObiParticleAttachment>();

        numberOfParticleAttachments = particleAttachments.Length;

        return true;

    }

    //Add static Attachments on the specific named group particles
    public void AddLock()
    {
        if (particleToLockStaticName == "None")
        {
            Debug.Log("Choose a name to search for in ObiActor and add static Obi Particle Attachments");
            return;
        }

        if (!Initialize(true))
            return;

        int nameCounter = 0;
        int attachmnetsCounter = 0;
        bool add = false;            

        for (int i = 0; i < particleGroups.Count; i++)
        {
            if (particleToLockStaticName == RemoveCharUntil(particleToLockStaticName, bluePrint.path.GetName(i)))
            {
                nameCounter++;
                string particleGroupName = particleToLockStaticName + i;
                bluePrint.path.SetName(i, particleGroupName);

                numberOfParticleAttachments = particleAttachments.Length;

                if (particleAttachments.Length == 0)
                    add = true;
                else
                {
                    int j = 0;

                    for(; j < numberOfParticleAttachments; j++) 
                    {
                        if (particleAttachments[j].particleGroup == particleGroups[i]) 
                        {
                            add = false;
                            break;
                        }
                    }
                       
                    if(j == numberOfParticleAttachments)
                        add = true;
                }

                if (add)
                {
                    var attachment = rope.AddComponent<ObiParticleAttachment>();

                    attachment.target = objectToAttach.transform;
                    attachment.particleGroup = particleGroups[i];
                    attachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
                    attachmnetsCounter++;
                }
            }
        }

        if (nameCounter == 0)
        {
            Debug.Log("Don't have any particle group with this name to put lock attachment");
            return;
        }

        else if (attachmnetsCounter == 0)
        {
            Debug.Log("All attachments for the lock particles with this name is already putted");
            return;
        }

        Debug.Log("Attachments added to lock particles!");
    }

    //Add dynamic Attachments on the specific named group particles
    public void AddBreak()
    {
        if (particleToLockDynamicName == "None")
        {
            Debug.Log("Choose a name to search for in ObiActor and add dynamic Obi Particle Attachments");
            return;
        }

        if (!Initialize(true))
            return;

        int nameCounter = 0;
        int attachmnetsCounter = 0;
        bool add = false;

        for (int i = 0; i < particleGroups.Count; i++)
        {
            if (particleToLockDynamicName == RemoveCharUntil(particleToLockDynamicName, bluePrint.path.GetName(i)))
            {
                nameCounter++;
                string particleGroupName = particleToLockDynamicName + i;
                bluePrint.path.SetName(i, particleGroupName);

                numberOfParticleAttachments = particleAttachments.Length;

                if (particleAttachments.Length == 0)
                    add = true;
                else
                {
                    int j = 0;

                    for (; j < numberOfParticleAttachments; j++)
                    {
                        if (particleAttachments[j].particleGroup == particleGroups[i])
                        {
                            add = false;
                            break;
                        }
                    }

                    if (j == numberOfParticleAttachments)
                        add = true;
                }

                if (add)
                {
                    var attachment = rope.AddComponent<ObiParticleAttachment>();

                    attachment.target = objectToAttach.transform;
                    attachment.particleGroup = particleGroups[i];
                    attachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
                    attachment.compliance = compliance;
                    attachment.breakThreshold = breakThreshold;
                    attachmnetsCounter++;
                }
            }
        }

        if (nameCounter == 0)
        {
            Debug.Log("Don't have any particle group with this name to put break attachment");
            return;
        }

        else if (attachmnetsCounter == 0)
        {
            Debug.Log("All attachments for the break particles with this name is already putted");
            return;
        }

        Debug.Log("Attachments added to break particles!");
    }

    //Remove static Attachments on the specific named group particles
    public void RemoveLocks()
    {
        if (!Initialize(false))
            return;

        int counter = 0;

        numberOfParticleAttachments = particleAttachments.Length;

        if (numberOfParticleAttachments == 0) 
        {
            Debug.Log("Don't have any attachment to erase");
            return;
        }


        for (int i = 0; i < numberOfParticleAttachments; i++)
        {
            ObiParticleAttachment attachment = particleAttachments[i];

            numberOfParticleAttachments = particleAttachments.Length;

            if (attachment.attachmentType == ObiParticleAttachment.AttachmentType.Static && particleToLockStaticName == RemoveCharUntil(particleToLockStaticName, attachment.particleGroup.name))
            {
                counter++;
                DestroyImmediate(attachment);
            }
        }

        if (counter == 0)
        {
            Debug.Log("Don't have any break attachment with this name to erase");
            return;
        }

        Debug.Log("Attachments to lock particles erased!");
    }

    //Remove dynamic Attachments on the specific named group particles
    public void RemoveBreaks()
    {
        if (!Initialize(false))
            return;

        int counter=0;

        numberOfParticleAttachments = particleAttachments.Length;

        if (numberOfParticleAttachments == 0)
        {
            Debug.Log("Don't have any attachment to erase");
            return;
        }

        for (int i = 0; i < numberOfParticleAttachments; i++)
        {
            ObiParticleAttachment attachment = particleAttachments[i];

            numberOfParticleAttachments = particleAttachments.Length;

            if (attachment.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic && particleToLockDynamicName == RemoveCharUntil(particleToLockDynamicName, attachment.particleGroup.name)) 
            {
                counter++;
                DestroyImmediate(attachment);
            }
                
        }

        if (counter == 0) 
        {
            Debug.Log("Don't have any break attachment with this name to erase");
            return;
        }

        Debug.Log("Attachments to break particles erased!");
    }

    //Remove all Attachments on the specific named group particles
    public void RemoveAll()
    {
        if (!Initialize(false))
            return;

        if (numberOfParticleAttachments == 0)
        {
            Debug.Log("Don't have any attachment to remove");
            return;
        }

        else 
        {
            RemoveLocks();
            RemoveBreaks();
        }
    }

    //Remove chars of string name until it a numbers of char equal to reference string
    private string RemoveCharUntil(string reference, string name)
    {
        int refereceSize = reference.Length;
        string newName = name;

        if (name.Length > refereceSize)
            newName = name.Remove(refereceSize);

        return newName;
    }
}



