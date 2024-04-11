using Obi;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(ObiActor))]
public class RecordedRope : RecordedObject
{
    [SerializeField] private GameObject startOfRope;
    [SerializeField] private GameObject endOfRope;
    public GameObject StartOfRope
    {
        get { return startOfRope; }
        set { startOfRope = value; }
    }
    public GameObject EndOfRope
    {
        get { return endOfRope; }
        set { endOfRope = value; }
    }

    #region Save Functions
    protected override IEnumerator GetObjectPosition()
    {
        if (startOfRope.tag == "EFL_Parent" && endOfRope.tag == "EFL_Parent")
        {
            Transform start1 = startOfRope.transform.GetChild(0);
            Transform start2 = startOfRope.transform.GetChild(1);

            Transform end1 = endOfRope.transform.GetChild(0);
            Transform end2 = endOfRope.transform.GetChild(1);

            record.RecordObjectStore.Add(new ObjectTransformToRecord(start1.localPosition, start1.localRotation));
            record.RecordObjectStore.Add(new ObjectTransformToRecord(start2.localPosition, start2.localRotation));
            record.RecordObjectStore.Add(new ObjectTransformToRecord(end1.localPosition, end1.localRotation));
            record.RecordObjectStore.Add(new ObjectTransformToRecord(end2.localPosition, end2.localRotation));
        }

        else
        {
            record.RecordObjectStore.Add(new ObjectTransformToRecord(startOfRope.transform.position, startOfRope.transform.rotation));
            record.RecordObjectStore.Add(new ObjectTransformToRecord(endOfRope.transform.position, endOfRope.transform.rotation));
        }

        for (int i = 0; i < rope.solverIndices.Length; i++)
        {
            record.RecordObjectStore.Add(new ObjectTransformToRecord(rope.solver.positions[rope.solverIndices[i]], new Quaternion(0, 0, 0, 0)));
        }

        yield return null;
    }

    #endregion

    #region Load Functions
    protected override IEnumerator LoadNewPathCoroutine(string path)
    {
        if (path == null) { yield return StartCoroutine(ChooseFile()); }

        else { folderPath = FindCorrectFileOnFolder(path); }

        yield return StartCoroutine(ReadPathFromFile());

        if (record.Name != rope.sourceBlueprint.name)
        {
            Debug.Log("Tried to load a wrong file to this object");
            loading = false;
        }

        else
        {
            if (record.RecordObjectStore.Count == 0)
            {
                Debug.Log("Problem Loading object position from File");
                loading = false;
            }

            else { LoadPositions(); }

            positionHelper.SelectedObject = null;
        }
    }
    protected override void LoadPositions()
    {
        int offset = 0;

        ObiParticleAttachment[] attach = rope.gameObject.GetComponents<ObiParticleAttachment>();
        List<ObiParticleGroup> groups = rope.sourceBlueprint.groups;
        for (int i = 0; i < attach.Length; i++)
        {
            if (attach[i].particleGroup != groups[0] && attach[i].particleGroup != groups[1] &&
                attach[i].particleGroup != groups[groups.Count - 1] &&
                attach[i].particleGroup != groups[groups.Count - 2])
            {
                Destroy(attach[i]);
            }
        }

        if (startOfRope.tag == "EFL_Parent" && endOfRope.tag == "EFL_Parent")
        {
            GameObject start1 = startOfRope.transform.GetChild(0).gameObject;
            GameObject start2 = startOfRope.transform.GetChild(1).gameObject;

            GameObject end1 = endOfRope.transform.GetChild(0).gameObject;
            GameObject end2 = endOfRope.transform.GetChild(1).gameObject;

            start1.GetComponent<Rigidbody>().isKinematic = true;
            end1.GetComponent<Rigidbody>().isKinematic = true;
            start2.GetComponent<Rigidbody>().isKinematic = true;
            end2.GetComponent<Rigidbody>().isKinematic = true;

            float s1ConnectedMassScale = start1.GetComponent<FixedJoint>().connectedMassScale;
            float e1ConnectedMassScale = end1.GetComponent<FixedJoint>().connectedMassScale;

            Destroy(start1.GetComponent<FixedJoint>());
            Destroy(end1.GetComponent<FixedJoint>());

            start1.transform.localPosition = record.RecordObjectStore[0].Postion;
            start1.transform.localRotation = record.RecordObjectStore[0].Rotation;
            start2.transform.localPosition = record.RecordObjectStore[1].Postion;
            start2.transform.localRotation = record.RecordObjectStore[1].Rotation;

            end1.transform.localPosition = record.RecordObjectStore[2].Postion;
            end1.transform.localRotation = record.RecordObjectStore[2].Rotation;
            end2.transform.localPosition = record.RecordObjectStore[3].Postion;
            end2.transform.localRotation = record.RecordObjectStore[3].Rotation;

            FixedJoint startJoint = start1.AddComponent<FixedJoint>();
            startJoint.connectedMassScale = s1ConnectedMassScale;

            FixedJoint endJoint = end1.AddComponent<FixedJoint>();
            endJoint.connectedMassScale = e1ConnectedMassScale;

            start1.GetComponent<Rigidbody>().isKinematic = false;
            end1.GetComponent<Rigidbody>().isKinematic = false;
            start2.GetComponent<Rigidbody>().isKinematic = false;
            end2.GetComponent<Rigidbody>().isKinematic = false;

            offset = 3;
        }

        else
        {
            startOfRope.transform.position = record.RecordObjectStore[0].Postion;
            startOfRope.transform.rotation = record.RecordObjectStore[0].Rotation;
            endOfRope.transform.position = record.RecordObjectStore[1].Postion;
            endOfRope.transform.rotation = record.RecordObjectStore[1].Rotation;

            offset = 1;
        }

        for (int i = 0; i < rope.blueprint.activeParticleCount; i++)
        {
            float invMasses = rope.solver.invMasses[rope.solverIndices[i]];
            rope.solver.invMasses[rope.solverIndices[i]] = 0;

            rope.solver.positions[rope.solverIndices[i]] = record.RecordObjectStore[offset + i].Postion;
            rope.solver.invMasses[rope.solverIndices[i]] = invMasses;
        }

        folderPath = null;
        loading = false;
    }

    #endregion

    #region Support Functions
    protected override void SaveOrLoadSetup()
    {
        base.SaveOrLoadSetup();
        record.Name = rope.sourceBlueprint.name;
    }
    protected override string FindCorrectFileOnFolder(string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfos = info.GetFiles();

        foreach (FileInfo fileInfo in fileInfos)
        {
            StreamReader sr = new StreamReader(fileInfo.FullName);
            string jsonstring = sr.ReadToEnd();
            sr.Close();

            record = JsonUtility.FromJson<RecordedObjectInfo>(jsonstring);
            if (record.Name == rope.sourceBlueprint.name)
            {
                path = fileInfo.FullName;
                return path;
            }
        }

        return null;
    }

    #endregion
}
