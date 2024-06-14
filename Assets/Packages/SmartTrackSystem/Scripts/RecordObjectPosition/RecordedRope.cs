using Obi;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SmartTrackSystem
{
    [RequireComponent(typeof(ObiActor))]
    public class RecordedRope : RecordedObject
    {
        public Transform startOfRope;
        public Transform endOfRope;
        public class Attachments
        {
            public Transform target = null;
            public ObiParticleGroup particleGroup = null;
            public ObiParticleAttachment.AttachmentType attachmentType = ObiParticleAttachment.AttachmentType.Static;
            public float compliance = 0.0f;
            public float breakThreshold = float.PositiveInfinity;
        }

        List<Attachments> destroyedAttachments = new List<Attachments>();

        #region Save Functions
        public override void GetObjectPosition()
        {
            bool initialIndex = true;

            int particleCount = rope.activeParticleCount;
            float lenght = 0.0f;

            if (rope.GetComponent<ObiRope>() != null)
            {
                lenght = rope.GetComponent<ObiRope>().restLength;
            }

            if(startOfRope != null) 
            {
                if (startOfRope.tag == "EFL_Parent")
                {
                    Transform start1 = startOfRope.GetChild(0);
                    Transform start2 = startOfRope.GetChild(1);

                    record.RecordObjectStore.Add(new ObjectTransformToRecord
                        (start1.gameObject.activeSelf,
                        start1.localPosition, 
                        start1.localRotation));
                    record.RecordObjectStore.Add(new ObjectTransformToRecord
                        (start2.gameObject.activeSelf,
                        start2.localPosition, 
                        start2.localRotation));

                    initialIndex = false;
                }

                else
                {
                    record.RecordObjectStore.Add(new ObjectTransformToRecord
                        (startOfRope.gameObject.activeSelf,
                        startOfRope.localPosition, 
                        startOfRope.localRotation));

                    initialIndex = false;
                }
            }

            ObiSolver solver = rope.solver;

            for (int i = 0; i < particleCount; i++)
            {
                record.RecordRopeStore.Add(new RopeTransformToRecord
                (initialIndex, gameObject.activeSelf,
                rope.solver.positions[rope.solverIndices[i]],
                rope.solver.orientations[rope.solverIndices[i]],
                solver.invMasses[rope.solverIndices[i]],
                solver.invRotationalMasses[rope.solverIndices[i]],
                particleCount, lenght));

                if (i == 0) { initialIndex = false; }
            }

            if(endOfRope != null) 
            {
                if (endOfRope.tag == "EFL_Parent")
                {
                    Transform end1 = endOfRope.transform.GetChild(0);
                    Transform end2 = endOfRope.transform.GetChild(1);

                    record.RecordObjectStore.Add(new ObjectTransformToRecord
                        (end1.gameObject.activeSelf, 
                        end1.localPosition,
                        end1.localRotation));
                    record.RecordObjectStore.Add(new ObjectTransformToRecord
                        (end2.gameObject.activeSelf, 
                        end2.localPosition,
                        end2.localRotation));
                }

                else
                {
                    record.RecordObjectStore.Add(new ObjectTransformToRecord
                        (endOfRope.gameObject.activeSelf,
                        endOfRope.localPosition, 
                        endOfRope.localRotation));
                }
            }
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

                else { LoadPositions(true); }

                positionHelper.SelectedObject = null;
            }
        }
        public override void LoadPositions(bool makePhysic)
        {
            ObiParticleAttachment[] attach = rope.GetComponents<ObiParticleAttachment>();

            for (int i = 0; i < attach.Length; i++)
            {
                SaveAttach(attach[i]);
                Destroy(attach[i]);
            }

            ObiRope obiRope = rope.GetComponent<ObiRope>();
            if (obiRope != null && obiRope.restLength !=
                record.RecordRopeStore[index].l)
            {
                ExtendRope(obiRope);
            }

            int particleCount = record.RecordRopeStore[index].pC;

            if (startOfRope != null)
            {
                if (startOfRope.tag == "EFL_Parent")
                {
                    Transform start1 = startOfRope.GetChild(0);
                    Transform start2 = startOfRope.GetChild(1);

                    start1.GetComponent<Rigidbody>().isKinematic = true;
                    start2.GetComponent<Rigidbody>().isKinematic = true;

                    start1.gameObject.SetActive(record.RecordObjectStore[index].e);
                    SetLocalPositionAndRotation(start1,
                    record.RecordObjectStore[index].p,
                    record.RecordObjectStore[index].r);
                    start2.gameObject.SetActive(record.RecordObjectStore[index + 1].e);
                    SetLocalPositionAndRotation(start1,
                    record.RecordObjectStore[index + 1].p,
                    record.RecordObjectStore[index + 1].r);

                    if (makePhysic)
                    {
                        start1.GetComponent<Rigidbody>().isKinematic = false;
                        start2.GetComponent<Rigidbody>().isKinematic = false;
                    }

                    index += 2;
                }

                else
                {
                    startOfRope.gameObject.SetActive(record.RecordObjectStore[index].e);
                    SetLocalPositionAndRotation(startOfRope,
                        record.RecordObjectStore[index].p,
                        record.RecordObjectStore[index].r);

                    index++;
                }
            }

            rope.gameObject.SetActive(record.RecordRopeStore[index].e);
            for (int i = 0; i < particleCount; i++)
            {
                rope.solver.invMasses[rope.solverIndices[i]] = 0;

                rope.solver.positions[rope.solverIndices[i]] =
                    record.RecordRopeStore[index + i].p;
                rope.solver.orientations[rope.solverIndices[i]] =
                    record.RecordRopeStore[index + i].r;

                if (makePhysic)
                {
                    rope.solver.invMasses[rope.solverIndices[i]] =
                        record.RecordRopeStore[index + i].iPM;
                    rope.solver.invRotationalMasses[rope.solverIndices[i]] =
                        record.RecordRopeStore[index + i].iRM;
                }
            }

            index += particleCount;

            if (endOfRope != null)
            {
                if (endOfRope.tag == "EFL_Parent")
                {
                    Transform end1 = endOfRope.GetChild(0);
                    Transform end2 = endOfRope.GetChild(1);

                    end1.GetComponent<Rigidbody>().isKinematic = true;
                    end2.GetComponent<Rigidbody>().isKinematic = true;

                    end1.gameObject.SetActive(record.RecordObjectStore[index].e);
                    SetLocalPositionAndRotation(end1,
                    record.RecordObjectStore[index].p,
                    record.RecordObjectStore[index].r);
                    end2.gameObject.SetActive(record.RecordObjectStore[index+1].e);
                    SetLocalPositionAndRotation(end2,
                    record.RecordObjectStore[index+1].p,
                    record.RecordObjectStore[index+1].r);

                    if (makePhysic)
                    {
                        end1.GetComponent<Rigidbody>().isKinematic = false;
                        end2.GetComponent<Rigidbody>().isKinematic = false;
                    }

                    index += 2;
                }

                else
                {
                    endOfRope.gameObject.SetActive(record.RecordObjectStore[index].e);
                    SetLocalPositionAndRotation(endOfRope,
                    record.RecordObjectStore[index].p,
                    record.RecordObjectStore[index].r);

                    index++;
                }
            }

            if (makePhysic) { ReturnAttachments(); }

            folderPath = null;
            loading = false;
        }

        #endregion

        #region Support Functions
        private void ExtendRope(ObiRope rope)
        {
            ObiRopeCursor cursor = rope.GetComponent<ObiRopeCursor>();

            if (cursor != null){
                cursor.ChangeLength(record.RecordRopeStore[index].l);
            }
        }
        private void SaveAttach(ObiParticleAttachment attach)
        {
            Attachments saveAttach = new Attachments();

            saveAttach.target = attach.target;
            saveAttach.particleGroup = attach.particleGroup;
            saveAttach.attachmentType = attach.attachmentType;

            if (saveAttach.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic)
            {
                saveAttach.compliance = attach.compliance;
                saveAttach.breakThreshold = attach.breakThreshold;
            }

            destroyedAttachments.Add(saveAttach);
        }
        public void ReturnAttachments()
        {
            foreach (Attachments attach in destroyedAttachments)
            {
                ObiParticleAttachment newAttach = rope.gameObject.AddComponent<ObiParticleAttachment>();

                newAttach.target = attach.target;
                newAttach.particleGroup = attach.particleGroup;
                newAttach.attachmentType = attach.attachmentType;

                if (newAttach.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic)
                {
                    newAttach.compliance = attach.compliance;
                    newAttach.breakThreshold = attach.breakThreshold;
                }
            }
        }
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
}
