using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        public override void GetObjectPosition(ref RecordedObjectInfo rec)
        {
            bool initialIndex = true;

            int particleCount = rope.activeParticleCount;
            float lenght = rope.GetComponent<ObiRopeBase>().restLength;

            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            if (startOfRope != null) 
            {
                if (startOfRope.tag == "EFL_Parent")
                {
                    Transform start1 = startOfRope.GetChild(0);
                    Transform start2 = startOfRope.GetChild(1);

                    rec.RecordRopeStore.Add(new RopeTransformToRecord
                        (initialIndex,start1.gameObject.activeSelf,
                        start1.localPosition.ToString(decimalPlaces, formatProvider), 
                        start1.localRotation.ToString(decimalPlaces, formatProvider)));
                    rec.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,start2.gameObject.activeSelf,
                        start2.localPosition.ToString(decimalPlaces, formatProvider), 
                        start2.localRotation.ToString(decimalPlaces, formatProvider)));

                    initialIndex = false;
                }

                else
                {
                    rec.RecordRopeStore.Add(new RopeTransformToRecord
                        (initialIndex,startOfRope.gameObject.activeSelf,
                        startOfRope.localPosition.ToString(decimalPlaces,formatProvider), 
                        startOfRope.localRotation.ToString(decimalPlaces,formatProvider)));

                    initialIndex = false;
                }
            }

            ObiSolver solver = rope.solver;

            for (int i = 0; i < particleCount; i++)
            {
                rec.RecordRopeStore.Add(new RopeTransformToRecord
                (initialIndex,gameObject.activeSelf,
                rope.solver.positions[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                rope.solver.orientations[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                solver.invMasses[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                solver.invRotationalMasses[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                particleCount, lenght.ToString(decimalPlaces,formatProvider)));

                if (i == 0) { initialIndex = false; }
            }

            if(endOfRope != null) 
            {
                if (endOfRope.tag == "EFL_Parent")
                {
                    Transform end1 = endOfRope.transform.GetChild(0);
                    Transform end2 = endOfRope.transform.GetChild(1);

                    rec.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,end1.gameObject.activeSelf, 
                        end1.localPosition.ToString(decimalPlaces,formatProvider),
                        end1.localRotation.ToString(decimalPlaces,formatProvider)));
                    rec.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,end2.gameObject.activeSelf, 
                        end2.localPosition.ToString(decimalPlaces,formatProvider),
                        end2.localRotation.ToString(decimalPlaces,formatProvider)));
                }

                else
                {
                    rec.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,endOfRope.gameObject.activeSelf,
                        endOfRope.localPosition.ToString(decimalPlaces,formatProvider), 
                        endOfRope.localRotation.ToString(decimalPlaces,formatProvider)));
                }
            }
        }
        protected override IEnumerator SaveNewPositionCoroutine(string path)
        {
            if (path == null) { yield return StartCoroutine(ChooseDirectory()); }

            else
            {
                path = CreateDirectoryToSaveAll(path);
                folderPath = FixFolderPath(path);
            }

            GetObjectPosition(ref recordPosition);

            if (recordPosition.RecordRopeStore.Count == 0)
            {
                Debug.Log("Problem Getting object position");
                saving = false;
            }

            else { SavePositions(); }
        }
        #endregion

        #region Load Functions
        protected override IEnumerator LoadNewPositionCoroutine(string path)
        {
            if (path == null) { yield return StartCoroutine(ChooseFile()); }

            else { folderPath = FindCorrectFileOnFolder(path); }

            yield return StartCoroutine(ReadPathFromFile());

            if (recordPosition.Name != rope.sourceBlueprint.name)
            {
                Debug.Log("Tried to load a wrong file to this object");
                loading = false;
            }

            else
            {
                if (recordPosition.RecordRopeStore.Count == 0)
                {
                    Debug.Log("Problem Loading object position from File");
                    loading = false;
                }

                else { LoadPositions(ref recordPosition,true); }

                smartTrack.SelectedObject = null;
            }
        }
        public override void LoadPositions(ref RecordedObjectInfo rec,bool makePhysic)
        {
            int j = 0;
            if(rec == record) {
                j = index;
            }

            ObiParticleAttachment[] attach = rope.GetComponents<ObiParticleAttachment>();

            for (int i = 0; i < attach.Length; i++){
                SaveAttach(attach[i]);
                Destroy(attach[i]);
            }

            float lenght = 0;
            if(startOfRope && startOfRope.tag == "EFL_Parent") {
                StringToFloat(rec.RecordRopeStore[j + 2].l);
            }

            else { StringToFloat(rec.RecordRopeStore[j + 1].l); }

            ObiRope obiRope = rope.GetComponent<ObiRope>();
            if (obiRope != null && obiRope.restLength != lenght){
                ExtendRope(obiRope,lenght);
            }

            if (startOfRope != null)
            {
                if (startOfRope.tag == "EFL_Parent")
                {
                    Transform handle = startOfRope.GetChild(0);
                    Transform body = startOfRope.GetChild(1);

                    handle.GetComponent<Rigidbody>().isKinematic = true;

                    handle.gameObject.SetActive(rec.RecordRopeStore[j].e);

                    SetLocalPositionAndRotation(handle,
                        StringToVector3(rec.RecordRopeStore[j].p),
                        StringToQuaternion(rec.RecordRopeStore[j].r));

                    body.gameObject.SetActive(rec.RecordRopeStore[j + 1].e);

                    SetLocalPositionAndRotation(body,
                        StringToVector3(rec.RecordRopeStore[j + 1].p),
                        StringToQuaternion(rec.RecordRopeStore[j + 1].r));

                    if (makePhysic)
                    {
                        DestroyImmediate(handle.GetComponent<FixedJoint>());
                        handle.GetComponent<Rigidbody>().isKinematic = false;
                        handle.gameObject.AddComponent<FixedJoint>();
                    }

                    j += 2;
                }

                else
                {
                    startOfRope.gameObject.SetActive(rec.RecordRopeStore[j].e);

                    SetLocalPositionAndRotation(startOfRope,
                        StringToVector3(rec.RecordRopeStore[j].p),
                        StringToQuaternion(rec.RecordRopeStore[j].r));

                    j++;
                }
            }

            int particleCount = rec.RecordRopeStore[j].pC;

            rope.gameObject.SetActive(rec.RecordRopeStore[j].e);
            for (int i = 0; i < particleCount; i++)
            {
                rope.solver.invMasses[rope.solverIndices[i]] = 0;

                rope.solver.positions[rope.solverIndices[i]] = 
                    StringToVector4(rec.RecordRopeStore[j + i].p);

                rope.solver.orientations[rope.solverIndices[i]] = 
                   StringToQuaternion(rec.RecordRopeStore[j + i].r);

                if (makePhysic)
                {
                    rope.solver.invMasses[rope.solverIndices[i]] =
                       StringToFloat(rec.RecordRopeStore[j + i].iPM);

                    rope.solver.invRotationalMasses[rope.solverIndices[i]] =
                       StringToFloat(rec.RecordRopeStore[j + i].iRM);
                }
            }

            j += particleCount;

            if (endOfRope != null)
            {
                if (endOfRope.tag == "EFL_Parent")
                {
                    Transform handle = endOfRope.GetChild(0);
                    Transform body = endOfRope.GetChild(1);

                    handle.GetComponent<Rigidbody>().isKinematic = true;

                    handle.gameObject.SetActive(rec.RecordRopeStore[j].e);

                    SetLocalPositionAndRotation(handle,
                        StringToVector3(rec.RecordRopeStore[j].p),
                        StringToQuaternion(rec.RecordRopeStore[j].r));

                    body.gameObject.SetActive(rec.RecordRopeStore[j + 1].e);

                    SetLocalPositionAndRotation(body,
                        StringToVector3(rec.RecordRopeStore[j + 1].p),
                        StringToQuaternion(rec.RecordRopeStore[j + 1].r));

                    if (makePhysic)
                    {
                        DestroyImmediate(handle.GetComponent<FixedJoint>());
                        handle.GetComponent<Rigidbody>().isKinematic = false;
                        handle.gameObject.AddComponent<FixedJoint>();
                    }

                    j += 2;
                }

                else
                {
                    endOfRope.gameObject.SetActive(rec.RecordRopeStore[j].e);

                    SetLocalPositionAndRotation(endOfRope,
                        StringToVector3(rec.RecordRopeStore[j].p),
                        StringToQuaternion(rec.RecordRopeStore[j].r));

                    j++;
                }
            }

            index = j;

            if (makePhysic) { ReturnAttachments(); }

            folderPath = null;
            loading = false;
        }

        #endregion

        #region Support Functions
        private void ExtendRope(ObiRope rope,float lenght)
        {
            ObiRopeCursor cursor = rope.GetComponent<ObiRopeCursor>();

            if (cursor != null){
                cursor.ChangeLength(lenght);
            }
        }
        private float StringToFloat(string lenght) 
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            NumberStyles style = NumberStyles.Float;

            float.TryParse(lenght.Trim(), style, formatProvider,out float result);

            return result;
        }
        private Vector4 StringToVector4(string rotation)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            string[] axis = rotation.Split(",");

            float x = float.Parse(axis[0].Replace("(", "").Trim(), formatProvider);
            float y = float.Parse(axis[1].Trim(), formatProvider);
            float z = float.Parse(axis[2].Trim(), formatProvider);
            float w = float.Parse(axis[3].Replace(")", "").Trim(), formatProvider);

            return new Vector4(x, y, z, w);
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
            List<ObiParticleGroup> groups = rope.sourceBlueprint.groups;

            foreach (Attachments attach in destroyedAttachments)
            {
                if (attach.particleGroup == groups[0] || 
                    attach.particleGroup == groups[1] || 
                    attach.particleGroup == groups[groups.Count-1] || 
                    attach.particleGroup == groups[groups.Count-2]) 
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
